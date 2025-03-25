using System.Text.Json.Serialization;

namespace TranslationApiClient.Data.DTOs;

internal sealed record TranslationRequestDto
{
    [JsonPropertyName("text_to_translate")]
    public string TextToTranslate { get; init; } = string.Empty;

    [JsonPropertyName("source_language")]
    public string SourceLanguage { get; init; } = string.Empty;

    [JsonPropertyName("target_language")]
    public string? TargetLanguage { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("generation_parameters")]
    public Dictionary<string, object>? GenerationParameters { get; init; }
}
