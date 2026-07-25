using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Domain.Entities;

public class TranslationSubmission
{
    public Guid Id { get; private set; }
    public Guid UserLanguageProfileId { get; private set; }
    public SubmissionMode Mode { get; private set; }
    public string OriginalInput { get; private set; } = string.Empty;
    public string? CorrectedOutput { get; private set; } = string.Empty;
    public string? GeneralFeedback { get; private set; } = string.Empty;

    public SubmissionStatus Status { get; private set; }
    public DateTime? ProcessedAt { get; private set; }
    public string? FailureReason { get; private set; }
    public string? TraceId { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public UserLanguageProfile UserLanguageProfile { get; private set; } = null!;

    private readonly List<SubmitTag> _submitTags = [];
    public IReadOnlyCollection<SubmitTag> SubmitTags => _submitTags.AsReadOnly();

    private TranslationSubmission() { }

    public static TranslationSubmission Create(
        Guid userLanguageProfileId,
        SubmissionMode mode,
        string originalInput)
    {
        if (userLanguageProfileId == Guid.Empty)
            throw new ArgumentException("El ID de UserLanguageProfile no puede estar vacío.", nameof(userLanguageProfileId));

        if (string.IsNullOrWhiteSpace(originalInput))
            throw new ArgumentException("El texto original es requerido.", nameof(originalInput));
        return new TranslationSubmission
        {
            Id = Guid.NewGuid(),
            UserLanguageProfileId = userLanguageProfileId,
            Mode = mode,
            OriginalInput = originalInput.Trim(),
            Status = SubmissionStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void MarkAsProcessing()
    {
        if (Status == SubmissionStatus.Completed || Status == SubmissionStatus.Processing)
        {
            throw new InvalidOperationException($"A request cannot be marked as processing while in status '{Status}'.");
        }
        Status = SubmissionStatus.Processing;
    }

    public void MarkAsCompleted(string correctedOutput, string generalFeedback)
    {
        if (Status != SubmissionStatus.Processing && Status != SubmissionStatus.Pending)
        {
            throw new InvalidOperationException($"\"A request cannot be marked as processing while in status '{Status}'.");
        }
        if (string.IsNullOrWhiteSpace(correctedOutput))
            throw new ArgumentException("The corrected output cannot be empty when marking as completed.", nameof(correctedOutput));
        if (string.IsNullOrWhiteSpace(generalFeedback))
            throw new ArgumentException("The general feedback cannot be empty when marking as completed.", nameof(generalFeedback));
        
        Status = SubmissionStatus.Completed;
        CorrectedOutput = correctedOutput.Trim();
        GeneralFeedback = generalFeedback.Trim();
        ProcessedAt = DateTime.UtcNow;
        FailureReason = null; 
    }

    public void MarkAsFailed(string failureReason)
    {
        if (string.IsNullOrWhiteSpace(failureReason))
            throw new ArgumentException("The failure reason is required.", nameof(failureReason));

        Status = SubmissionStatus.Failed;
        FailureReason = failureReason.Trim();
        ProcessedAt = DateTime.UtcNow;
    }


    public void SetTraceId(string traceId)
    {
        if (!string.IsNullOrWhiteSpace(traceId))
        {
            TraceId = traceId.Trim();
        }
    }

}
