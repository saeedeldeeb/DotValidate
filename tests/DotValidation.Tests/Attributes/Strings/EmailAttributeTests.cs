using DotValidation.Attributes.Strings;
using Xunit;

namespace DotValidation.Tests.Attributes.Strings;

public class EmailAttributeTests
{
    private readonly EmailAttribute _attribute = new();

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name@domain.org")]
    [InlineData("user+tag@example.co.uk")]
    public void IsValid_ValidEmails_ReturnsTrue(string email)
    {
        Assert.True(_attribute.IsValid(email));
    }

    [Theory]
    [InlineData("invalid")]
    [InlineData("@example.com")]
    [InlineData("test@")]
    [InlineData("test@.com")]
    public void IsValid_InvalidEmails_ReturnsFalse(string email)
    {
        Assert.False(_attribute.IsValid(email));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        Assert.True(_attribute.IsValid(""));
    }
}
