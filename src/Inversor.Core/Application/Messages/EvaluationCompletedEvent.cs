using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Application.Messages;

/// <summary>
/// Event published by the Worker when an evaluation finishes (Success or Failure).
/// </summary>
public record EvaluationCompletedEvent(
    Guid SubmissionId,
    Guid UserLanguageProfileId,
    SubmissionStatus Status,
    string? CorrectedOutput,
    string? GeneralFeedback,
    string? FailureReason,
    string? SignalRConnectionId // It will use in the 2 phase for ephemeral Guests
);