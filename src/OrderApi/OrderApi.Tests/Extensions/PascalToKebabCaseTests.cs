using FluentAssertions;
using OrderApi.Messaging.Extensions;

namespace OrderApi.Tests.Extensions;

public class PascalToKebabCaseTests
{
    [Theory]
    [InlineData("HelloWorld", "hello-world")]
    [InlineData("PascalCase", "pascal-case")]
    [InlineData("JSONData", "json-data")]
    [InlineData("UserID", "user-id")]
    [InlineData("HTTPRequest", "http-request")]
    [InlineData("A", "a")]
    [InlineData("AB", "ab")]
    [InlineData("Already-Kebab", "already-kebab")]
    public void ShouldConvertCorrectly(string input, string expected)
    {
        // Act
        var result = input.PascalToKebabCase();

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void ShouldReturnEmpty_WhenInputIsEmpty()
    {
        // Arrange
        const string input = "";

        // Act
        var result = input.PascalToKebabCase();

        // Assert
        result.Should().BeEmpty();
    }
}