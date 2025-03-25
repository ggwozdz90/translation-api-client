using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using TranslationApiClient.Adapter.Adapters;
using TranslationApiClient.Application.UseCases;
using TranslationApiClient.Data.DataSources;
using TranslationApiClient.Data.Repositories;
using TranslationApiClient.Domain.Repositories;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.DependencyInjection;

/// <summary>
///     This class is responsible for registering internal services in the external container in the application that will
///     use this library.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds the TranslationAdapter service to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the service to.</param>
    /// <param name="configuration">The configuration to use.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddTranslationProcessor(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDataLayer(configuration);
        services.AddDomainLayer();
        services.AddApplicationLayer();
        services.AddAdapterLayer();

        return services;
    }

    private static void AddDataLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var apiBaseAddress = configuration.GetValue("Translation:BaseAddress", "http://localhost:8000");
        var translateRouteTimeout = configuration.GetValue("Translation:TranslateRouteTimeout", 300);
        var healthCheckRouteTimeout = configuration.GetValue("Translation:HealthCheckRouteTimeout", 10);

        services
            .AddRefitClient<ITranslationRemoteDataSource>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(apiBaseAddress);
                c.Timeout = TimeSpan.FromSeconds(translateRouteTimeout);
            });

        services
            .AddRefitClient<IHealthCheckRemoteDataSource>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(apiBaseAddress);
                c.Timeout = TimeSpan.FromSeconds(healthCheckRouteTimeout);
            });

        services.AddTransient<ITranslationRepository, TranslationRepository>();
    }

    private static void AddDomainLayer(this IServiceCollection services)
    {
        services.AddTransient<ITranslationService, TranslationService>();
        services.AddTransient<IHealthCheckService, HealthCheckService>();
    }

    private static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddTransient<ITranslateTextUseCase, TranslateTextUseCase>();
        services.AddTransient<IHealthCheckUseCase, HealthCheckUseCase>();
    }

    private static void AddAdapterLayer(this IServiceCollection services)
    {
        services.AddTransient<ITranslationAdapter, TranslationAdapter>();
    }
}
