namespace Inversor.Core.Application.DTOs.Request;

public class EvaluateTextRequest
{
    public Guid UserLanguageProfileId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Mode { get; set; } = "Free";
}
