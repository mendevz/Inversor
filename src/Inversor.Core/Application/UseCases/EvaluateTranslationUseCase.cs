using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.AiEvaluator;
using Inversor.Core.Application.DTOs.Request;
using Inversor.Core.Domain.Entities;
using Inversor.Core.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inversor.Core.Application.UseCases;

public class EvaluateTranslationUseCase(
    IApplicationDbContext dbContext,
    IAiEvaluatorService aiService,
    ILogger<EvaluateTranslationUseCase> logger)
{
    public async Task<EvaluationResponseDto> ExecuteAsync(EvaluateTextRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Evaluating translation for user language profile {UserLanguageProfileId}", request.UserLanguageProfileId);

        if (string.IsNullOrWhiteSpace(request.Text))
            throw new ValidationException("Text to evaluate cannot be empty.");

        var profile = await dbContext.UserLanguageProfiles
            .FirstOrDefaultAsync(p => p.Id == request.UserLanguageProfileId, cancellationToken)
            ?? throw new NotFoundException("Language profile not found.");

        profile.TrackNewRequest();
        if (profile.DailyRequestCount > 20)
        {
            throw new ValidationException("You have reached the daily request limit evaluations.");
        }

        logger.LogInformation("Calling AI evaluator. Assessing level: {Level}", profile.AssessedLevel);

        var validTags = await dbContext.GrammarTopics
            .Select(g => g.Tag)
            .ToListAsync(cancellationToken);

        var availableTagsString = string.Join(", ", validTags);

        var aiResult = await aiService.EvaluateTextAsync(
            userInput: request.Text,
            nativeLang: profile.NativeLanguageCode,
            learnLang: profile.LearnLanguageCode,
            userLevel: profile.AssessedLevel,
            availableTags: availableTagsString,
            cancellationToken: cancellationToken);

        if (aiResult.SecurityAlert)
        {
            throw new ValidationException("Prompt injection detected. Request refused");
        }

        var submission = TranslationSubmission.Create(
            userLanguageProfileId: profile.Id,
            mode: request.Mode,
            originalInput: request.Text);

        //,
        //    correctedOutput: aiResult.CorrectedText ?? string.Empty,
        //    generalFeedback: aiResult.GeneralFeedback ?? string.Empty

        dbContext.TranslationSubmissions.Add(submission);

        await ProcessTagsAndMasteryAsync(
            profile.Id,
            submission.Id,
            aiResult.Analysis,
            cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Translation evaluation completed. TranslationSubmissionId: {TranslationSubmissionId}", submission.Id);

        return aiResult;
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

                logger.LogInformation("LLM invented an unknown tag: {InventedTag}. Redirecting to {FallbackTag}",
                          topicTag, fallbackTag);

                grammarTopic = await dbContext.GrammarTopics
                    .FirstOrDefaultAsync(g => g.Tag == fallbackTag, cancellationToken);

                if (grammarTopic == null)
                {
                    logger.LogWarning("Critical failure: Tag {Tag} or its fallback {Fallback} not found.", topicTag, fallbackTag);
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

            if (mastery == null)
            {
                mastery = TopicMastery.Create(profileId, grammarTopic.Id);
                dbContext.TopicMasteries.Add(mastery);
            }

            mastery.RecordAttempt(item.IsError);
        }
    }

}
