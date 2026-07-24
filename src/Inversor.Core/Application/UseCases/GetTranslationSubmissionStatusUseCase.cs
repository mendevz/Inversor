using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.Response;
using Inversor.Core.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Inversor.Core.Application.UseCases;

public class GetTranslationSubmissionStatusUseCase(IApplicationDbContext dbContext)
{
    /// <summary>
    /// Retrieves current status and details of a submission for recovery or fallback querying.
    /// </summary>
    public async Task<TranslationSubmissionDetailDto> ExecuteAsync(Guid submissionId, CancellationToken cancellationToken)
    {
        var submission = await dbContext.TranslationSubmissions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == submissionId, cancellationToken)
            ?? throw new NotFoundException($"Translation submission with ID {submissionId} was not found.");

        return new TranslationSubmissionDetailDto(
            Id: submission.Id,
            UserLanguageProfileId: submission.UserLanguageProfileId,
            Mode: submission.Mode,
            OriginalInput: submission.OriginalInput,
            Status: submission.Status,
            CorrectedOutput: submission.CorrectedOutput,
            GeneralFeedback: submission.GeneralFeedback,
            FailureReason: submission.FailureReason,
            CreatedAt: submission.CreatedAt,
            ProcessedAt: submission.ProcessedAt
        );
    }
}
