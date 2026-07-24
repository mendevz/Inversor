using Google.GenAI;
using Google.GenAI.Types;
using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.AiEvaluator;
using Inversor.Infrastructure.AI.Prompts;
using Inversor.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Inversor.Infrastructure.AI.Services;

public class GeminiEvaluatorService(
    ILogger<GeminiEvaluatorService> logger,
    IOptions<GeminiOptions> geminiOptions) : IAiEvaluatorService
{

    private readonly GeminiOptions _options = geminiOptions.Value;

    private readonly ResiliencePipeline _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                // We target transient failures: 503 Service Unavailable, 429 Too Many Requests, high demand, timeouts.
                ShouldHandle = new PredicateBuilder().Handle<Exception>(ex =>
                    ex.Message.Contains("high demand", StringComparison.OrdinalIgnoreCase) ||
                    ex.Message.Contains("429") ||
                    ex.Message.Contains("503") ||
                    ex.Message.Contains("timeout", StringComparison.OrdinalIgnoreCase)),

                Delay = TimeSpan.FromSeconds(geminiOptions.Value.DelaySeconds),
                MaxRetryAttempts = geminiOptions.Value.MaxRetries,
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                OnRetry = args =>
                {
                    logger.LogWarning(args.Outcome.Exception,
                        "Gemini API transient failure. Retrying... Attempt {AttemptNumber} of {MaxRetries} after {Delay}ms",
                        args.AttemptNumber + 1, geminiOptions.Value.MaxRetries, args.RetryDelay.TotalMilliseconds);

                    return default;
                }
            })
            .Build();

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<EvaluationResponseDto> EvaluateTextAsync(
        string userInput, 
        string nativeLang, 
        string learnLang, 
        string userLevel,
        string availableTags,
        CancellationToken cancellationToken)
    {           
        try
        {
            var client = new Client(apiKey: _options.ApiKey);
            var systemInstruction = SystemPrompts.GetEvaluatorPrompt(nativeLang, learnLang, userLevel, availableTags);

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts =
                    [
                        new Part { Text = systemInstruction }
                    ]
                },
                Temperature = _options.Temperature,
                ResponseMimeType = "application/json",
                MaxOutputTokens = _options.MaxOutputTokens
            };

            logger.LogInformation("Sending request to Gemini AI for evaluation.");

            var response = await _resiliencePipeline.ExecuteAsync(async ct =>
            {
                return await client.Models.GenerateContentAsync(
                    model: _options.Model, 
                    contents: userInput, 
                    config: config, 
                    cancellationToken: ct);

            }, cancellationToken);

            var jsonResult = response.Text;

            if (string.IsNullOrWhiteSpace(jsonResult))
                throw new Exception("Gemini AI retrived a empty response.");

            jsonResult = jsonResult.Trim();
            if (jsonResult.StartsWith("```json"))
                jsonResult = jsonResult.Replace("```json", "").Replace("```", "").Trim();
            else if (jsonResult.StartsWith("```"))
                jsonResult = jsonResult.Replace("```", "").Trim();

            try
            {
                var jsonNode = JsonNode.Parse(jsonResult) ?? throw new Exception("El nodo JSON devuelto es nulo.");
                var evaluation = jsonNode.Deserialize<EvaluationResponseDto>(_jsonOptions);
                return evaluation ?? throw new Exception("Error al mapear el nodo JSON al DTO.");
            }
            catch (JsonException jsonEx)
            {
                logger.LogWarning(jsonEx, "El JSON del LLM vino mal estructurado. Intentando normalización agresiva.");

                var sanitizedJson = jsonResult
                    .Replace(" '", " \"")
                    .Replace("' ", "\" ")
                    .Replace("('", "(\"")
                    .Replace("')", "\")");
                try
                {
                    var secondaryNode = JsonNode.Parse(sanitizedJson);
                    var evaluation = secondaryNode?.Deserialize<EvaluationResponseDto>(_jsonOptions);
                    if (evaluation != null) return evaluation;
                }
                catch
                {
                    logger.LogError("Fallo absoluto al intentar reparar el payload del LLM. Raw JSON: {Raw}", jsonResult);
                }

                throw new Exception("La IA devolvió un formato de datos incompatible con el sistema.", jsonEx);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during text evaluation with Gemini AI.");    
            throw;
        }
        
    }
}
