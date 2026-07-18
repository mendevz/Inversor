namespace Inversor.Core.Domain.Entities;

public class SubmitTag
{
    public Guid Id { get; private set; }
    public Guid TranslationSubmissionId { get; private set; }
    public Guid GrammarTopicId { get; private set; }
    public bool IsError { get; private set; }
    public string OriginalFragment { get; private set; } = string.Empty;
    public string CorrectedFragment { get; private set; } = string.Empty;
    public string BriefExplanation { get; private set; } = string.Empty;

    public TranslationSubmission TranslationSubmission { get; private set; } = null!;
    public GrammarTopic GrammarTopic { get; private set; } = null!;

    private SubmitTag() { }

    public static SubmitTag Create(
            Guid translationSubmissionId,
            Guid grammarTopicId,
            bool isError,
            string originalFragment,
            string correctedFragment,
            string briefExplanation
        )
    {
        if (translationSubmissionId == Guid.Empty) throw new ArgumentException("TranslationSubmission ID cannot be empty", nameof(translationSubmissionId));
        if (grammarTopicId == Guid.Empty) throw new ArgumentException("GrammarTopic ID cannot be empty", nameof(grammarTopicId));
        if (string.IsNullOrWhiteSpace(originalFragment)) throw new ArgumentException("Original fragment is required", nameof(originalFragment));
        if (string.IsNullOrWhiteSpace(correctedFragment)) throw new ArgumentException("Corrected fragment is required", nameof(correctedFragment));
        if (string.IsNullOrWhiteSpace(briefExplanation)) throw new ArgumentException("Brief explanation is required", nameof(briefExplanation));
        var submitTag = new SubmitTag
        {
            Id = Guid.NewGuid(),
            TranslationSubmissionId = translationSubmissionId,
            GrammarTopicId = grammarTopicId,
            IsError = isError,
            OriginalFragment = originalFragment.Trim(),
            CorrectedFragment = correctedFragment.Trim(),
            BriefExplanation = briefExplanation.Trim()
        };
        return submitTag;
    }
}
