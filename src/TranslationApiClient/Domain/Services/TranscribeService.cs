using Microsoft.Extensions.Logging;
using TranslationApiClient.Domain.Exceptions;
using TranslationApiClient.Domain.Repositories;

namespace TranslationApiClient.Domain.Services;

internal interface ITranslationService
{
    Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage);
}

internal sealed class TranslationService(
    ILogger<TranslationService> logger,
    ITranslationRepository translationRepository
) : ITranslationService
{
    public async Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace(
            "Translating the text '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} invoked from service...",
            textToTranslate,
            sourceLanguage,
            targetLanguage
        );

        try
        {
            return await translationRepository
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is NetworkException or TranslationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "An error occurred while translating the text '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} from service...",
                textToTranslate,
                sourceLanguage,
                targetLanguage
            );
            throw;
        }
    }
}
