using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using TranslationApiClient.Adapter.Adapters;
using TranslationApiClient.Application.UseCases;

namespace TranslationApiClient.Tests.Adapter.Adapters;

[TestFixture]
internal sealed class TranslationAdapterTests
{
    private ILogger<TranslationAdapter> logger = null!;
    private ITranslateTextUseCase translateTextUseCase = null!;
    private IHealthCheckUseCase healthCheckUseCase = null!;
    private TranslationAdapter adapter = null!;

    [SetUp]
    public void Setup()
    {
        logger = Substitute.For<ILogger<TranslationAdapter>>();
        translateTextUseCase = Substitute.For<ITranslateTextUseCase>();
        healthCheckUseCase = Substitute.For<IHealthCheckUseCase>();
        adapter = new TranslationAdapter(logger, translateTextUseCase, healthCheckUseCase);
    }

    [Test]
    public async Task TranslateAsync_ShouldReturnResult_WhenNoExceptionOccursAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en_US";
        const string targetLanguage = "es_ES";
        const string expectedResult = "Translated text";
        translateTextUseCase.InvokeAsync(textToTranslate, sourceLanguage, targetLanguage).Returns(expectedResult);

        // When
        var result = await adapter
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        result.Should().Be(expectedResult);
        await translateTextUseCase
            .Received(1)
            .InvokeAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task HealthCheckAsync_ShouldReturnOk_WhenNoExceptionOccursAsync()
    {
        // Given
        const string expectedResult = "OK";
        healthCheckUseCase.InvokeAsync().Returns(expectedResult);

        // When
        var result = await adapter.HealthCheckAsync().ConfigureAwait(false);

        // Then
        result.Should().Be(expectedResult);
        await healthCheckUseCase.Received(1).InvokeAsync().ConfigureAwait(false);
    }
}
