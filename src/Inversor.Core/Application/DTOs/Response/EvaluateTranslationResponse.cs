using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Application.DTOs.Response;

/// <summary>
/// Lightweight DTO returned immediately after enqueuing a translation evaluation request (HTTP 202 Accepted).
/// </summary>
public record EvaluateTranslationResponseDto(
    Guid SubmissionId,
    SubmissionStatus Status,
    DateTime CreatedAt
);
