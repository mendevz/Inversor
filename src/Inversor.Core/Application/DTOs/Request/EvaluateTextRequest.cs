using Inversor.Core.Domain.Enums;

namespace Inversor.Core.Application.DTOs.Request;

public class EvaluateTextRequest
{
    public Guid UserLanguageProfileId { get; set; }
    public string Text { get; set; } = string.Empty;
    public SubmissionMode Mode { get; set; } = SubmissionMode.FREE;
}
