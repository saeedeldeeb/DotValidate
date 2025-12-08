using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class UuidAttributeTests
{
    private readonly UuidAttribute _attribute = new();

    // Valid UUIDs in various formats
    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("550E8400-E29B-41D4-A716-446655440000")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    [InlineData("ffffffff-ffff-ffff-ffff-ffffffffffff")]
    [InlineData("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF")]
    [InlineData("{550e8400-e29b-41d4-a716-446655440000}")]
    [InlineData("(550e8400-e29b-41d4-a716-446655440000)")]
    [InlineData("550e8400e29b41d4a716446655440000")]
    public void IsValid_ValidUuid_ReturnsTrue(string value)
    {
        Assert.True(_attribute.IsValid(value));
    }

    // Invalid UUIDs
    [Theory]
    [InlineData("not-a-uuid")]
    [InlineData("550e8400-e29b-41d4-a716")]
    [InlineData("550e8400-e29b-41d4-a716-44665544000g")]
    [InlineData("550e8400-e29b-41d4-a716-4466554400000")]
    [InlineData("550e8400e29b41d4a71644665544000")]
    [InlineData("12345")]
    [InlineData("xyz")]
    public void IsValid_InvalidUuid_ReturnsFalse(string value)
    {
        Assert.False(_attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        Assert.False(_attribute.IsValid(123));
    }

    [Fact]
    public void IsValid_GuidToString_ReturnsTrue()
    {
        var guid = Guid.NewGuid().ToString();
        Assert.True(_attribute.IsValid(guid));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var message = _attribute.FormatErrorMessage("TransactionId");
        Assert.Equal("TransactionId must be a valid UUID.", message);
    }
}
