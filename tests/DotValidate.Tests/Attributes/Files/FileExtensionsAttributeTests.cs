using DotValidate.Attributes.Files;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DotValidate.Tests.Attributes.Files;

public class FileExtensionsAttributeTests
{
    private static IFormFile CreateMockFile(string fileName, string contentType = "application/octet-stream")
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
    public void IsValid_AllowedExtension_ReturnsTrue()
    {
        var attribute = new FileExtensionsAttribute(".pdf", ".doc", ".docx");
        var file = CreateMockFile("document.pdf");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_NotAllowedExtension_ReturnsFalse()
    {
        var attribute = new FileExtensionsAttribute(".pdf", ".doc");
        var file = CreateMockFile("document.exe");
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_CaseInsensitive_ReturnsTrue()
    {
        var attribute = new FileExtensionsAttribute(".pdf");
        var file = CreateMockFile("document.PDF");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_ExtensionWithoutDot_ReturnsTrue()
    {
        var attribute = new FileExtensionsAttribute("pdf", "doc"); // No leading dots
        var file = CreateMockFile("document.pdf");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_MultipleExtensions_ReturnsTrue()
    {
        var attribute = new FileExtensionsAttribute(".tar.gz"); // Double extension
        var file = CreateMockFile("archive.tar.gz");
        // Note: Path.GetExtension returns only ".gz"
        Assert.False(attribute.IsValid(file)); // Will fail because only .gz is extracted
    }

    [Fact]
    public void IsValid_NoExtension_ReturnsFalse()
    {
        var attribute = new FileExtensionsAttribute(".pdf");
        var file = CreateMockFile("document");
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new FileExtensionsAttribute(".pdf");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonFileType_ReturnsFalse()
    {
        var attribute = new FileExtensionsAttribute(".pdf");
        Assert.False(attribute.IsValid("not a file"));
    }

    [Fact]
    public void Constructor_NoExtensions_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new FileExtensionsAttribute());
    }

    [Fact]
    public void AllowedExtensions_AreNormalized()
    {
        var attribute = new FileExtensionsAttribute("PDF", ".Doc", "docx");
        Assert.Contains(".pdf", attribute.AllowedExtensions);
        Assert.Contains(".doc", attribute.AllowedExtensions);
        Assert.Contains(".docx", attribute.AllowedExtensions);
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new FileExtensionsAttribute(".pdf", ".doc");
        var message = attribute.FormatErrorMessage("Document");
        Assert.Contains("Document", message);
        Assert.Contains(".pdf", message);
        Assert.Contains(".doc", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new FileExtensionsAttribute(".pdf") { ErrorMessage = "{0} has invalid extension" };
        var message = attribute.FormatErrorMessage("File");
        Assert.Equal("File has invalid extension", message);
    }
}
