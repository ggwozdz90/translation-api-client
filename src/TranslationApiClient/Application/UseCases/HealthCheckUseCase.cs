using Microsoft.Extensions.Logging;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.Application.UseCases;

internal interface IHealthCheckUseCase
{
    Task<string> InvokeAsync();
}

internal sealed class HealthCheckUseCase(ILogger<HealthCheckUseCase> logger, IHealthCheckService healthCheckService)
    : IHealthCheckUseCase
{
    public async Task<string> InvokeAsync()
    {
        logger.LogTrace("Health check invoked from use case...");

        var result = await healthCheckService.HealthCheckAsync().ConfigureAwait(false);

        return result;
    }
}
