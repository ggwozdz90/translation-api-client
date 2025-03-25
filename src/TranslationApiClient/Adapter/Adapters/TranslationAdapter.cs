using Microsoft.Extensions.Logging;
using TranslationApiClient.Application.UseCases;
using TranslationApiClient.Domain.Exceptions;

namespace TranslationApiClient.Adapter.Adapters;

/// <summary>
///     Interface for Translation adapter.
/// </summary>
public interface ITranslationAdapter
{
    /// <summary>
    ///     Sends a request to the API to translate the text to the specified language.
    ///     The language should be provided in the pattern xx_XX.
    /// </summary>
    /// <param name="textToTranslate">The text to be translated.</param>
    /// <param name="sourceLanguage">The source language of the text in the pattern xx_XX.</param>
    /// <param name="targetLanguage">The target language for translation in the pattern xx_XX.</param>
    /// <returns>
    ///     The task result contains the translated text.
    /// </returns>
    /// <exception cref="NetworkException">Thrown when an HTTP error occurs during the translation process.</exception>
    /// <exception cref="TranslationException">Thrown when an error occurs during the translation process.</exception>
    Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage);

    /// <summary>
    ///     Checks the health of the API.
    /// </summary>
    /// <returns>The task result contains the health status, which will be "OK" if the API is healthy.</returns>
    /// <exception cref="NetworkException">Thrown when an HTTP error occurs during the health check process.</exception>
    /// <exception cref="HealthCheckException">Thrown when an error occurs during the health check process.</exception>
    Task<string> HealthCheckAsync();
}

internal sealed class TranslationAdapter(
    ILogger<TranslationAdapter> logger,
    ITranslateTextUseCase translateTextUseCase,
    IHealthCheckUseCase healthCheckUseCase
) : ITranslationAdapter
{
    public async Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage)
    {
        logger.LogInformation(
            "Translate invoked for text: {TextToTranslate}, source language: {SourceLanguage}, and target language: {TargetLanguage}",
            textToTranslate,
            sourceLanguage,
            targetLanguage
        );

        var result = await translateTextUseCase
            .InvokeAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        logger.LogInformation(
            "Translate completed for text: {TextToTranslate}, source language: {SourceLanguage}, target language: {TargetLanguage} with result: {Result}",
            textToTranslate,
            sourceLanguage,
            targetLanguage,
            result
        );

        return result;
    }

    public async Task<string> HealthCheckAsync()
    {
        logger.LogInformation("Health check invoked...");

        var result = await healthCheckUseCase.InvokeAsync().ConfigureAwait(false);

        logger.LogInformation("Health check completed with result: {Result}", result);

        return result;
    }
}
