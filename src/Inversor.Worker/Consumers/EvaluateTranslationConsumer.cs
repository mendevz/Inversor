using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.AiEvaluator;
using Inversor.Core.Application.Messages;
using Inversor.Core.Domain.Entities;
using Inversor.Core.Domain.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Inversor.Worker.Consumers;

public class EvaluateTranslationConsumer(
    IApplicationDbContext dbContext,
    IAiEvaluatorService aiService,
    ILogger<EvaluateTranslationConsumer> logger) : IConsumer<EvaluateTranslationCommand>
{
    public async Task Consume(ConsumeContext<EvaluateTranslationCommand> context)
    {
        var command = context.Message;
        logger.LogInformation("Processing evaluation submission {SubmissionId}", command.SubmissionId);

        var submission = await dbContext.TranslationSubmissions
            .FirstOrDefaultAsync(s => s.Id == command.SubmissionId, context.CancellationToken);

        if (submission is null)
        {
            logger.LogWarning("Submission {SubmissionId} not found in database. Skipping message.", command.SubmissionId);
            return;
        }

        // 2. Strict Idempotency & Concurrency Guard
        if (submission.Status is SubmissionStatus.Completed or SubmissionStatus.Processing)
        {
            logger.LogInformation("Submission {SubmissionId} is in state '{Status}'. Skipping execution to prevent duplicate LLM charges.",
                command.SubmissionId, submission.Status);
            return;
        }


        var profile = await dbContext.UserLanguageProfiles
            .FirstOrDefaultAsync(p => p.Id == command.UserLanguageProfileId, context.CancellationToken);

        if (profile is null)
        {
            logger.LogError("Profile {ProfileId} not found for submission {SubmissionId}.", command.UserLanguageProfileId, command.SubmissionId);
            submission.MarkAsFailed("User language profile not found.");
            await dbContext.SaveChangesAsync(context.CancellationToken);
            return;
        }

        try
        {
            submission.MarkAsProcessing();
            await dbContext.SaveChangesAsync(context.CancellationToken);

            // Fetch available grammar tags for LLM context
            var validTags = await dbContext.GrammarTopics
                .Select(g => g.Tag)
                .ToListAsync(context.CancellationToken);
            var availableTagsString = string.Join(", ", validTags);


            // Invoke AI Evaluator Service
            var aiResult = await aiService.EvaluateTextAsync(
                userInput: command.Text,
                nativeLang: profile.NativeLanguageCode,
                learnLang: profile.LearnLanguageCode,
                userLevel: profile.AssessedLevel,
                availableTags: availableTagsString,
                cancellationToken: context.CancellationToken);

            if (aiResult.SecurityAlert)
            {
                logger.LogWarning("Prompt injection security alert for submission {SubmissionId}.", command.SubmissionId);
                submission.MarkAsFailed("Prompt injection detected. Request refused.");
                await dbContext.SaveChangesAsync(context.CancellationToken);
                return;
            }

            // Process analysis tags and SRS mastery
            await ProcessTagsAndMasteryAsync(
                profile.Id,
                submission.Id,
                aiResult.Analysis,
                context.CancellationToken);


            // Transition state to Completed
            submission.MarkAsCompleted(
                correctedOutput: aiResult.CorrectedText ?? string.Empty,
                generalFeedback: aiResult.GeneralFeedback ?? string.Empty
            );

            await dbContext.SaveChangesAsync(context.CancellationToken);
            logger.LogInformation("Successfully completed evaluation submission {SubmissionId}.", command.SubmissionId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to evaluate submission {SubmissionId}.", command.SubmissionId);
            submission.MarkAsFailed(ex.Message);
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }
    }

    private async Task ProcessTagsAndMasteryAsync(
        Guid profileId,
        Guid submissionId,
        List<EvaluationAnalysisDto> analysis,
        CancellationToken cancellationToken)
    {
        foreach (var item in analysis)
        {
            var topicTag = item.ConceptTag.ToLower().Trim();
            var grammarTopic = await dbContext.GrammarTopics
                .FirstOrDefaultAsync(g => g.Tag == topicTag, cancellationToken);
            if (grammarTopic is null)
            {
                var fallbackTag = $"{item.MacroCategory.ToLower()}_general_error";
                logger.LogInformation("LLM invented unknown tag: {InventedTag}. Redirecting to fallback {FallbackTag}",
                    topicTag, fallbackTag);
                grammarTopic = await dbContext.GrammarTopics
                    .FirstOrDefaultAsync(g => g.Tag == fallbackTag, cancellationToken);
                if (grammarTopic is null)
                {
                    logger.LogWarning("Tag {Tag} or fallback {Fallback} not found.", topicTag, fallbackTag);
                    continue;
                }
            }
            var submitTag = SubmitTag.Create(
                translationSubmissionId: submissionId,
                grammarTopicId: grammarTopic.Id,
                isError: item.IsError,
                originalFragment: item.OriginalFragment,
                correctedFragment: item.CorrectedFragment,
                briefExplanation: item.BriefExplanation);
            dbContext.SubmitTags.Add(submitTag);
            var mastery = await dbContext.TopicMasteries
                .FirstOrDefaultAsync(t => t.UserLanguageProfileId == profileId && t.GrammarTopicId == grammarTopic.Id, cancellationToken);
            if (mastery is null)
            {
                mastery = TopicMastery.Create(profileId, grammarTopic.Id);
                dbContext.TopicMasteries.Add(mastery);
            }
            mastery.RecordAttempt(item.IsError);
        }
    }
}
