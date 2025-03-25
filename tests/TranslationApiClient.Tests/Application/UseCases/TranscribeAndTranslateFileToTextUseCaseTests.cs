using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using TranslationApiClient.Application.UseCases;
using TranslationApiClient.Domain.Services;

namespace TranslationApiClient.Tests.Application.UseCases;

[TestFixture]
internal sealed class TranslateTextUseCaseTests
{
    private ILogger<TranslateTextUseCase> logger = null!;
    private ITranslationService translationService = null!;
    private TranslateTextUseCase useCase = null!;

    [SetUp]
    public void SetUp()
    {
        logger = Substitute.For<ILogger<TranslateTextUseCase>>();
        translationService = Substitute.For<ITranslationService>();
        useCase = new TranslateTextUseCase(logger, translationService);
    }

    [Test]
    public async Task InvokeAsync_ShouldReturnTranslatedText_WhenNoExceptionOccursAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        const string expectedText = "translated text";
        translationService
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .Returns(Task.FromResult(expectedText));

        // When
        var result = await useCase.InvokeAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        result.Should().Be(expectedText);
        await translationService
            .Received(1)
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
    }

    [Test]
    public async Task InvokeAsync_ShouldThrowException_WhenTranslateFailsAsync()
    {
        // Given
        const string textToTranslate = "test";
        const string sourceLanguage = "en";
        const string targetLanguage = "fr";
        translationService
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ThrowsAsync(new InvalidOperationException("Translate failed"));

        // When
        Func<Task> act = async () =>
            await useCase.InvokeAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

        // Then
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("Translate failed")
            .ConfigureAwait(false);
        await translationService
            .Received(1)
            .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
            .ConfigureAwait(false);
        logger
            .Received(1)
            .Log(
                LogLevel.Trace,
                Arg.Any<EventId>(),
                Arg.Is<object>(v =>
                    v.ToString() == "Translating the text: 'test' from en to fr invoked from use case..."
                ),
                exception: null,
                Arg.Any<Func<object, Exception?, string>>()
            );
    }
}
