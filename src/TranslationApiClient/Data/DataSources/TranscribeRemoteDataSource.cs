using Refit;
using TranslationApiClient.Data.DTOs;

namespace TranslationApiClient.Data.DataSources;

internal interface ITranslationRemoteDataSource
{
    [Post("/translate")]
    Task<TranslationDto> TranslateAsync([Body] TranslationRequestDto request);
}
