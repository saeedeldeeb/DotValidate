using DotValidation.Attributes.Files;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DotValidation.Tests.Attributes.Files;

public class MimeTypesAttributeTests
{
    private static IFormFile CreateMockFile(string contentType, string fileName = "test.txt")
    {
        var content = new byte[100];
        var stream = new MemoryStream(content);
        return new FormFile(stream, 0, content.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Fact]
    public void IsValid_AllowedMimeType_ReturnsTrue()
    {
        var attribute = new MimeTypesAttribute("application/pdf", "image/png");
        var file = CreateMockFile("application/pdf", "document.pdf");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_NotAllowedMimeType_ReturnsFalse()
    {
        var attribute = new MimeTypesAttribute("application/pdf");
        var file = CreateMockFile("application/msword", "document.doc");
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_CaseInsensitive_ReturnsTrue()
    {
        var attribute = new MimeTypesAttribute("application/pdf");
        var file = CreateMockFile("APPLICATION/PDF", "document.pdf");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_MimeTypeWithParameters_ReturnsTrue()
    {
        var attribute = new MimeTypesAttribute("text/plain");
        var file = CreateMockFile("text/plain; charset=utf-8", "document.txt");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new MimeTypesAttribute("application/pdf");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonFileType_ReturnsFalse()
    {
        var attribute = new MimeTypesAttribute("application/pdf");
        Assert.False(attribute.IsValid("not a file"));
    }

    [Fact]
    public void IsValid_EmptyContentType_ReturnsFalse()
    {
        var attribute = new MimeTypesAttribute("application/pdf");
        var file = CreateMockFile("", "document.pdf");
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void Constructor_NoMimeTypes_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new MimeTypesAttribute());
    }

    [Fact]
    public void AllowedMimeTypes_AreLowercased()
    {
        var attribute = new MimeTypesAttribute("Application/PDF", "IMAGE/PNG");
        Assert.Contains("application/pdf", attribute.AllowedMimeTypes);
        Assert.Contains("image/png", attribute.AllowedMimeTypes);
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new MimeTypesAttribute("application/pdf", "image/png");
        var message = attribute.FormatErrorMessage("Document");
        Assert.Contains("Document", message);
        Assert.Contains("application/pdf", message);
        Assert.Contains("image/png", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new MimeTypesAttribute("application/pdf") { ErrorMessage = "{0} has invalid type" };
        var message = attribute.FormatErrorMessage("File");
        Assert.Equal("File has invalid type", message);
    }
}
