using Refit;

namespace TranslationApiClient.Data.DataSources;

internal interface ITranslationRemoteDataSource
{
    [Multipart]
    [Post("/translate")]
    Task<string> TranslateAsync(
        [AliasAs("text_to_translate")] string textToTranslate,
        [AliasAs("source_language")] string sourceLanguage,
        [AliasAs("target_language")] string? targetLanguage = null,
        [AliasAs("translation_parameters")] string? translationParameters = null
    );
}
