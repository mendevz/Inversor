using System.ComponentModel.DataAnnotations;

namespace Inversor.Infrastructure.Options;

/// <summary>
/// Strongly-typed configuration options for Gemini AI integration.
/// </summary>
public class GeminiOptions
{
    public const string SectionName = "Gemini";

    [Required(ErrorMessage = "Gemini API Key is required.")]
    public string ApiKey { get; set; } = string.Empty;

    [Required(ErrorMessage = "Gemini Model name is required.")]
    public string Model { get; set; } = "gemini-2.5-flash";

    [Range(0.0, 1.0, ErrorMessage = "Temperature must be between 0.0 and 1.0")]
    public float Temperature { get; set; } = 0.1f;

    [Range(100, 8192, ErrorMessage = "MaxOutputTokens must be between 100 and 8192")]
    public int MaxOutputTokens { get; set; } = 4000;
}
