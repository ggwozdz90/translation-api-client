using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TranslationApiClient.Domain.Exceptions;
using TranslationApiClient.Domain.Repositories;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.Tests.Domain.Services;

[TestFixture]
internal sealed class TranslationServiceTests
{
    private ILogger<TranslationService> logger = null!;
    private ITranslationRepository repository = null!;
    private TranslationService service = null!;

    [SetUp]
    public void SetUp()
    {
        logger = Substitute.For<ILogger<TranslationService>>();
        repository = Substitute.For<ITranslationRepository>();
        service = new TranslationService(logger, repository);
    }

    [Test]
    public async Task TranslateAsync_ShouldReturnTranslation_WhenNoExceptionOccursAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string expectedTranslation = "Bonjour, le monde!";
        repository.TranslateAsync(textToTranslate, sourceLanguage, targetLanguage).Returns(expectedTranslation);

        // When
        var result = await service
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);

        // Then
        result.Should().Be(expectedTranslation);
    }

    [Test]
    public async Task TranslateAsync_ShouldThrowTranslationException_WhenRepositoryThrowsTranslationExceptionAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        repository
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ThrowsAsync(new TranslationException());

        // When
        Func<Task> act = async () =>
            await service.TranslateAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<TranslationException>().ConfigureAwait(false);
        await repository
            .Received(1)
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task TranslateAsync_ShouldLogErrorAndThrow_WhenRepositoryThrowsUnexpectedExceptionAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        repository
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ThrowsAsync(new InvalidOperationException());

        // When
        Func<Task> act = async () =>
            await service.TranslateAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should().ThrowAsync<InvalidOperationException>().ConfigureAwait(false);
        await repository
            .Received(1)
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }
}
