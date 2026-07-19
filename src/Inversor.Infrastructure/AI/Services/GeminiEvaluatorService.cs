using Google.GenAI;
using Google.GenAI.Types;
using Inversor.Core.Application.Abstractions;
using Inversor.Core.Application.DTOs.AiEvaluator;
using Inversor.Infrastructure.AI.Prompts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Inversor.Infrastructure.AI.Services;

public class GeminiEvaluatorService(
    ILogger<GeminiEvaluatorService> logger,
    IConfiguration configuration) : IAiEvaluatorService
{
    private readonly string _apiKey = configuration["Gemini:ApiKey"] ?? string.Empty;

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<EvaluationResponseDto> EvaluateTextAsync(
        string userInput, 
        string nativeLang, 
        string learnLang, 
        string userLevel,
        CancellationToken cancellationToken)
    {           
        try
        {
            var client = new Client(apiKey: _apiKey);
            var systemInstruction = SystemPrompts.GetEvaluatorPrompt(nativeLang, learnLang, userLevel);

            var config = new GenerateContentConfig
            {
                SystemInstruction = new Content
                {
                    Parts =
                    [
                        new Part { Text = systemInstruction }
                    ]
                },
                Temperature = 0.1f,
                ResponseMimeType = "application/json"
            };

            logger.LogInformation("Sending request to Gemini AI for evaluation.");

            var response = await client.Models.GenerateContentAsync(
                model: "gemini-3.5-flash",
                contents: userInput,
                config: config
            );

            var jsonResult = response.Text;

            if (string.IsNullOrWhiteSpace(jsonResult))
                throw new Exception("Gemini AI retrived a empty response.");

            var evaluation = JsonSerializer.Deserialize<EvaluationResponseDto>(jsonResult, _jsonOptions);

            return evaluation ?? throw new Exception("Error al deserializar la respuesta de Gemini.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during text evaluation with Gemini AI.");    
            throw;
        }
        
    }
}
