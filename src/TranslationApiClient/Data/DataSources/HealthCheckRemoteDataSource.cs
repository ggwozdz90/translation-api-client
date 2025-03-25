using Refit;
using TranslationApiClient.Data.DTOs;

namespace TranslationApiClient.Data.DataSources;

internal interface IHealthCheckRemoteDataSource
{
    [Get("/healthcheck")]
    Task<HealthCheckDto> HealthCheckAsync();
}
