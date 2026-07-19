namespace Inversor.Core.Application.DTOs.AiEvaluator;
public class EvaluationResponseDto
{
    public bool SecurityAlert { get; set; }
    public string? ErrorCode { get; set; }
    public string? Message { get; set; }
    public string? InternalThinking { get; set; }
    public string? GeneralFeedback { get; set; }
    public string? OriginalText { get; set; }
    public string? CorrectedText { get; set; }
    public List<EvaluationAnalysisDto> Analysis { get; set; } = [];
}
