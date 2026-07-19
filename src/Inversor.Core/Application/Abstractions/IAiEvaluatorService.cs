using Inversor.Core.Application.DTOs.AiEvaluator;

namespace Inversor.Core.Application.Abstractions;

public interface IAiEvaluatorService
{
    Task<EvaluationResponseDto> EvaluateTextAsync(string userInput, string nativeLang, string learnLang, string userLevel);
}
