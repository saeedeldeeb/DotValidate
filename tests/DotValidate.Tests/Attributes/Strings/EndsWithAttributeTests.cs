using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests.Attributes.Strings;

public class EndsWithAttributeTests
{
    [Theory]
    [InlineData("document.pdf", ".pdf", ".doc")]
    [InlineData("report.doc", ".pdf", ".doc")]
    [InlineData("image.png", ".png", ".jpg", ".gif")]
    [InlineData("photo.jpg", ".png", ".jpg", ".gif")]
    public void IsValid_EndsWithValidValue_ReturnsTrue(string value, params string[] allowed)
    {
        var attribute = new EndsWithAttribute(allowed);
        Assert.True(attribute.IsValid(value));
    }

    [Theory]
    [InlineData("document.txt", ".pdf", ".doc")]
    [InlineData("image.bmp", ".png", ".jpg", ".gif")]
    [InlineData("hello", "foo", "bar")]
    public void IsValid_DoesntEndWithAllowedValue_ReturnsFalse(string value, params string[] allowed)
    {
        var attribute = new EndsWithAttribute(allowed);
        Assert.False(attribute.IsValid(value));
    }

    [Fact]
    public void IsValid_CaseSensitive_DifferentCase_ReturnsFalse()
    {
        var attribute = new EndsWithAttribute(".pdf");
        Assert.False(attribute.IsValid("document.PDF"));
    }

    [Fact]
    public void IsValid_IgnoreCase_DifferentCase_ReturnsTrue()
    {
        var attribute = new EndsWithAttribute(".pdf", ".doc") { IgnoreCase = true };
        Assert.True(attribute.IsValid("document.PDF"));
        Assert.True(attribute.IsValid("report.DOC"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new EndsWithAttribute(".pdf");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_EmptyString_ReturnsTrue()
    {
        var attribute = new EndsWithAttribute(".pdf");
        Assert.True(attribute.IsValid(string.Empty));
    }

    [Fact]
    public void IsValid_NonString_ReturnsFalse()
    {
        var attribute = new EndsWithAttribute(".pdf");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_NoValues_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new EndsWithAttribute());
    }

    [Fact]
    public void FormatErrorMessage_IncludesAllowedValues()
    {
        var attribute = new EndsWithAttribute(".pdf", ".doc", ".docx");
        var message = attribute.FormatErrorMessage("Filename");
        Assert.Equal("Filename must end with one of the following: .pdf, .doc, .docx.", message);
    }
}
