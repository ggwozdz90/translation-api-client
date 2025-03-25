namespace TranslationApiClient.Domain.Repositories;

internal interface ITranslationRepository
{
    Task<string> TranslateAsync(string textToTranslate, string sourceLanguage, string targetLanguage);
    Task<string> HealthCheckAsync();
}
