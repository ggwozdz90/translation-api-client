using Microsoft.Extensions.Logging;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.Application.UseCases;

internal interface ITranslateTextUseCase
{
    Task<string> InvokeAsync(string textToTranslate, string sourceLanguage, string targetLanguage);
}

internal sealed class TranslateTextUseCase(ILogger<TranslateTextUseCase> logger, ITranslationService translationService)
    : ITranslateTextUseCase
{
    public async Task<string> InvokeAsync(string textToTranslate, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace(
            "Translating the text: '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} invoked from use case...",
            textToTranslate,
            sourceLanguage,
            targetLanguage
        );

        var result = await translationService
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        return result;
    }
}
