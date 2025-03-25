using Microsoft.Extensions.Logging;
using Refit;
using TranslationApiClient.Data.DataSources;
using TranslationApiClient.Domain.Exceptions;
using TranslationApiClient.Domain.Repositories;

namespace TranslationApiClient.Data.Repositories;

internal sealed class TranslationRepository(
    ILogger<TranslationRepository> logger,
    ITranslationRemoteDataSource translationRemoteDataSource,
    IHealthCheckRemoteDataSource healthCheckRemoteDataSource
) : ITranslationRepository
{
    public async Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage)
    {
        logger.LogTrace(
            "Translating the text '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} invoked from repository...",
            textToTranslate,
            sourceLanguage,
            targetLanguage
        );

        try
        {
            var result = await translationRemoteDataSource
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);

            return result;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(
                httpEx,
                "HTTP error occurred while translating the text '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} from repository...",
                textToTranslate,
                sourceLanguage,
                targetLanguage
            );
            throw new NetworkException(httpEx);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(
                apiEx,
                "API error occurred while translating the text '{TextToTranslate}' from {SourceLanguage} to {TargetLanguage} from repository...",
                textToTranslate,
                sourceLanguage,
                targetLanguage
            );
            throw new TranslationException(apiEx);
        }
    }

    public async Task<string> HealthCheckAsync()
    {
        logger.LogTrace("Health check invoked from repository...");

        try
        {
            var dto = await healthCheckRemoteDataSource.HealthCheckAsync().ConfigureAwait(false);

            return dto.Status;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP error occurred during health check from repository...");
            throw new NetworkException(httpEx);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "API error occurred during health check from repository...");
            throw new HealthCheckException(apiEx);
        }
    }
}
