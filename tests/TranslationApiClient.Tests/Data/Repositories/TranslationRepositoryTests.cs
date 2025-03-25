#pragma warning disable IDISP004, RCS1261, MA0042 // Don't ignore created IDisposable -> Justification: Analyzer is reporting issue even when stream is disposed in the test method.

using System.Net;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NUnit.Framework;
using Refit;
using TranslationApiClient.Data.DataSources;
using TranslationApiClient.Data.DTOs;
using TranslationApiClient.Data.Repositories;
using TranslationApiClient.Domain.Exceptions;

namespace TranslationApiClient.Tests.Data.Repositories
{
    [TestFixture]
    internal sealed class TranslationRepositoryTests
    {
        private ILogger<TranslationRepository> logger = null!;
        private ITranslationRemoteDataSource translationRemoteDataSource = null!;
        private IHealthCheckRemoteDataSource healthCheckRemoteDataSource = null!;
        private TranslationRepository repository = null!;

        [SetUp]
        public void SetUp()
        {
            logger = Substitute.For<ILogger<TranslationRepository>>();
            translationRemoteDataSource = Substitute.For<ITranslationRemoteDataSource>();
            healthCheckRemoteDataSource = Substitute.For<IHealthCheckRemoteDataSource>();
            repository = new TranslationRepository(logger, translationRemoteDataSource, healthCheckRemoteDataSource);
        }

        [Test]
        public async Task TranslateAsync_ShouldReturnTranslation_WhenNoExceptionOccursAsync()
        {
            // Given
            const string textToTranslate = "test";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            const string expectedTranslation = "Translation result";
            translationRemoteDataSource
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .Returns(expectedTranslation);

            // When
            var result = await repository
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);

            // Then
            result.Should().Be(expectedTranslation);
        }

        [Test]
        public async Task TranslateAsync_ShouldThrowNetworkException_WhenHttpRequestExceptionOccursAsync()
        {
            // Given
            const string textToTranslate = "test";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            translationRemoteDataSource
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ThrowsAsync(new HttpRequestException());

            // When
            Func<Task> act = async () =>
                await repository.TranslateAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
            await translationRemoteDataSource
                .Received(1)
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task TranslateAsync_ShouldThrowTranslationException_WhenApiExceptionOccursAsync()
        {
            // Given
            const string textToTranslate = "test";
            const string sourceLanguage = "en";
            const string targetLanguage = "fr";
            using var httpRequestMessage = new HttpRequestMessage();
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var exception = await ApiException
                .Create(httpRequestMessage, HttpMethod.Post, httpResponseMessage, new RefitSettings())
                .ConfigureAwait(false);
            translationRemoteDataSource
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ThrowsAsync(exception);

            // When
            Func<Task> act = async () =>
                await repository.TranslateAsync(textToTranslate, sourceLanguage, targetLanguage).ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<TranslationException>().ConfigureAwait(false);
            await translationRemoteDataSource
                .Received(1)
                .TranslateAsync(textToTranslate, sourceLanguage, targetLanguage)
                .ConfigureAwait(false);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldReturnHealthStatus_WhenNoExceptionOccursAsync()
        {
            // Given
            const string expectedStatus = "Healthy";
            var healthCheckDto = new HealthCheckDto(expectedStatus);
            healthCheckRemoteDataSource.HealthCheckAsync().Returns(healthCheckDto);

            // When
            var result = await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            result.Should().Be(expectedStatus);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldThrowNetworkException_WhenHttpRequestExceptionOccursAsync()
        {
            // Given
            healthCheckRemoteDataSource.HealthCheckAsync().ThrowsAsync(new HttpRequestException());

            // When
            Func<Task> act = async () => await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<NetworkException>().ConfigureAwait(false);
            await healthCheckRemoteDataSource.Received(1).HealthCheckAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task HealthCheckAsync_ShouldThrowHealthCheckException_WhenApiExceptionOccursAsync()
        {
            // Given
            using var httpRequestMessage = new HttpRequestMessage();
            using var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var exception = await ApiException
                .Create(httpRequestMessage, HttpMethod.Post, httpResponseMessage, new RefitSettings())
                .ConfigureAwait(false);
            healthCheckRemoteDataSource.HealthCheckAsync().ThrowsAsync(exception);

            // When
            Func<Task> act = async () => await repository.HealthCheckAsync().ConfigureAwait(false);

            // Then
            await act.Should().ThrowAsync<HealthCheckException>().ConfigureAwait(false);
            await healthCheckRemoteDataSource.Received(1).HealthCheckAsync().ConfigureAwait(false);
        }
    }
}

#pragma warning restore IDISP004, RCS1261, MA0042 // Don't ignore created IDisposable
