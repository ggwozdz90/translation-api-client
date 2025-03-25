using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;
using TranslationApiClient.Adapter.Adapters;
using TranslationApiClient.Application.UseCases;
using TranslationApiClient.Data.DataSources;
using TranslationApiClient.DependencyInjection;
using TranslationApiClient.Domain.Repositories;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.Tests.DependencyInjection;

[TestFixture]
internal sealed class ServiceCollectionExtensionsTests
{
    private IServiceCollection services = null!;
    private IConfiguration configuration = null!;

    [SetUp]
    public void Setup()
    {
        configuration = Substitute.For<IConfiguration>();

        var baseAddressConfigurationSection = Substitute.For<IConfigurationSection>();
        baseAddressConfigurationSection.Value.Returns("http://localhost:5000");
        configuration.GetSection("Translation:BaseAddress").Returns(baseAddressConfigurationSection);

        var translateRouteTimeoutConfigurationSection = Substitute.For<IConfigurationSection>();
        translateRouteTimeoutConfigurationSection.Value.Returns("300");
        configuration
            .GetSection("Translation:TranslateRouteTimeout")
            .Returns(translateRouteTimeoutConfigurationSection);

        var healthCheckRouteTimeoutConfigurationSection = Substitute.For<IConfigurationSection>();
        healthCheckRouteTimeoutConfigurationSection.Value.Returns("10");
        configuration
            .GetSection("Translation:HealthCheckRouteTimeout")
            .Returns(healthCheckRouteTimeoutConfigurationSection);

        services = new ServiceCollection();
        services.AddSingleton(configuration);
    }

    [Test]
    public void AddTranslationProcessor_ShouldRegisterTranslationAdapter()
    {
        // Given, When
        services.AddTranslationProcessor(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ITranslationAdapter>().Should().NotBeNull();
    }

    [Test]
    public void AddTranslationProcessor_ShouldRegisterDataLayer()
    {
        // Given, When
        services.AddTranslationProcessor(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ITranslationRemoteDataSource>().Should().NotBeNull();
        serviceProvider.GetService<IHealthCheckRemoteDataSource>().Should().NotBeNull();
        serviceProvider.GetService<ITranslationRepository>().Should().NotBeNull();
    }

    [Test]
    public void AddTranslationProcessor_ShouldRegisterDomainLayer()
    {
        // Given, When
        services.AddTranslationProcessor(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ITranslationService>().Should().NotBeNull();
        serviceProvider.GetService<IHealthCheckService>().Should().NotBeNull();
    }

    [Test]
    public void AddTranslationProcessor_ShouldRegisterApplicationLayer()
    {
        // Given, When
        services.AddTranslationProcessor(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ITranslateTextUseCase>().Should().NotBeNull();
        serviceProvider.GetService<IHealthCheckUseCase>().Should().NotBeNull();
    }

    [Test]
    public void AddTranslationProcessor_ShouldRegisterAdapterLayer()
    {
        // Given, When
        services.AddTranslationProcessor(configuration);
        using var serviceProvider = services.BuildServiceProvider();

        // Then
        serviceProvider.GetService<ITranslationAdapter>().Should().NotBeNull();
    }
}
