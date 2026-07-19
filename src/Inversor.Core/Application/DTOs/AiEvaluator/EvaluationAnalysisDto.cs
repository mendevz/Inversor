namespace Inversor.Core.Application.DTOs.AiEvaluator;

public class EvaluationAnalysisDto
{
    public string MacroCategory { get; set; } = string.Empty;
    public string ConceptTag { get; set; } = string.Empty;
    public string FriendlyTitle { get; set; } = string.Empty;
    public bool IsError { get; set; }
    public string OriginalFragment { get; set; } = string.Empty;
    public string CorrectedFragment { get; set; } = string.Empty;
    public string BriefExplanation { get; set; } = string.Empty;
}
