
using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Application.DTOs.Response;

/// <summary>
/// Detailed DTO returned when querying the current status/results of a translation submission.
/// </summary>
public record TranslationSubmissionDetailDto(
    Guid Id,
    Guid UserLanguageProfileId,
    SubmissionMode Mode,
    string OriginalInput,
    SubmissionStatus Status,
    string? CorrectedOutput,
    string? GeneralFeedback,
    string? FailureReason,
    DateTime CreatedAt,
    DateTime? ProcessedAt
);