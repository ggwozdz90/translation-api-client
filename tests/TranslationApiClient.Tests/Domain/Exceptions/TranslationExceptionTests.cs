using FluentAssertions;
using NUnit.Framework;
using TranslationApiClient.Domain.Exceptions;

namespace TranslationApiClient.Tests.Domain.Exceptions;

[TestFixture]
internal sealed class TranslationExceptionTests
{
    [Test]
    public void TranslationException_ShouldInitializeWithDefaultMessage()
    {
        // When
        var exception = new TranslationException();

        // Then
        exception.Message.Should().Be("An error occurred while translating the text.");
    }

    [Test]
    public void TranslationException_ShouldInitializeWithSpecifiedMessage()
    {
        // Given
        const string expectedMessage = "Custom error message.";

        // When
        var exception = new TranslationException(expectedMessage);

        // Then
        exception.Message.Should().Be(expectedMessage);
    }

    [Test]
    public void TranslationException_ShouldInitializeWithInnerException()
    {
        // Given
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new TranslationException(innerException);

        // Then
        exception.Message.Should().Be("An error occurred while translating the text.");
        exception.InnerException.Should().Be(innerException);
    }

    [Test]
    public void TranslationException_ShouldInitializeWithSpecifiedMessageAndInnerException()
    {
        // Given
        const string expectedMessage = "Custom error message.";
        var innerException = new InvalidOperationException("Inner exception message.");

        // When
        var exception = new TranslationException(expectedMessage, innerException);

        // Then
        exception.Message.Should().Be(expectedMessage);
        exception.InnerException.Should().Be(innerException);
    }
}
