using FluentAssertions;
using NUnit.Framework;
using TranslationApiClient.Data.DTOs;

namespace TranslationApiClient.Tests.Data.DTOs;

[TestFixture]
internal sealed class HealthCheckDtoTests
{
    [Test]
    public void HealthCheckDto_ShouldInitializeStatus()
    {
        // Given
        const string expectedStatus = "Healthy";

        // When
        var dto = new HealthCheckDto(expectedStatus);

        // Then
        dto.Status.Should().Be(expectedStatus);
    }
}
