namespace Inversor.Core.Domain.Entities;

public class TranslationSubmission
{
    public Guid Id { get; private set; }
    public Guid UserLanguageProfileId { get; private set; }
    public string Mode { get; private set; } = string.Empty;
    public string OriginalInput { get; private set; } = string.Empty;
    public string CorrectedOutput { get; private set; } = string.Empty;
    public string GeneralFeedback { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }
    public UserLanguageProfile UserLanguageProfile { get; private set; } = null!;

    private readonly List<SubmitTag> _submitTags = [];
    public IReadOnlyCollection<SubmitTag> SubmitTags => _submitTags.AsReadOnly();

    private TranslationSubmission() { }

    public static TranslationSubmission Create(
            Guid userLanguageId,
            string mode,
            string originalInput,
            string correctedOutput,
            string generalFeedback
        )
    {
        if (userLanguageId == Guid.Empty) throw new ArgumentException("UserLanguageProfile ID cannot be empty", nameof(userLanguageId));
        if (string.IsNullOrWhiteSpace(mode)) throw new ArgumentException("Mode is required", nameof(mode));
        if (string.IsNullOrWhiteSpace(originalInput)) throw new ArgumentException("Original input is required", nameof(originalInput));
        if (string.IsNullOrWhiteSpace(correctedOutput)) throw new ArgumentException("Corrected output is required", nameof(correctedOutput));
        if (string.IsNullOrWhiteSpace(generalFeedback)) throw new ArgumentException("General feedback is required", nameof(generalFeedback));
        var translationSubmission = new TranslationSubmission
        {
            Id = Guid.NewGuid(),
            UserLanguageProfileId = userLanguageId,
            Mode = mode.Trim(),
            OriginalInput = originalInput.Trim(),
            CorrectedOutput = correctedOutput.Trim(),
            GeneralFeedback = generalFeedback.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        return translationSubmission;
    }

}
