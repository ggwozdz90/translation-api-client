using Microsoft.Extensions.Logging;
using TranslationApiClient.Domain.Exceptions;
using TranslationApiClient.Domain.Repositories;

namespace TranslationApiClient.Domain.Services;

internal interface IHealthCheckService
{
    Task<string> HealthCheckAsync();
}

internal sealed class HealthCheckService(
    ILogger<HealthCheckService> logger,
    ITranslationRepository translationRepository
) : IHealthCheckService
{
    public async Task<string> HealthCheckAsync()
    {
        logger.LogTrace("Health check invoked from service...");

        try
        {
            return await translationRepository.HealthCheckAsync().ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is NetworkException or HealthCheckException)
        {
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during health check from service...");
            throw;
        }
    }
}
