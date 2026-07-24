using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.Request;
using Inversor.Core.Application.DTOs.Response;
using Inversor.Core.Application.Messages;
using Inversor.Core.Domain.Entities;
using Inversor.Core.Domain.Exceptions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Inversor.Core.Application.UseCases;

public class EvaluateTranslationUseCase(
    IApplicationDbContext dbContext,
    IPublishEndpoint publishEndpoint,
    ILogger<EvaluateTranslationUseCase> logger)
{
    public async Task<EvaluateTranslationResponseDto> ExecuteAsync(EvaluateTranslationRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Receiving evaluation request for user language profile {UserLanguageProfileId}", request.UserLanguageProfileId);

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

        var submission = TranslationSubmission.Create(
            userLanguageProfileId: profile.Id,
            mode: request.Mode,
            originalInput: request.Text);

        dbContext.TranslationSubmissions.Add(submission);

        var command = new EvaluateTranslationCommand(
            SubmissionId: submission.Id,
            UserLanguageProfileId: profile.Id,
            Mode: request.Mode,
            Text: request.Text
        );

        await publishEndpoint.Publish(command, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Enqueued translation submission {SubmissionId} to RabbitMQ.", submission.Id);

        return new EvaluateTranslationResponseDto(
            SubmissionId: submission.Id,
            Status: submission.Status,
            CreatedAt: submission.CreatedAt
        );
    }
}
