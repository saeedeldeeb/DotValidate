using DotValidation.Attributes.Files;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DotValidation.Tests.Attributes.Files;

public class FileSizeAttributeTests
{
    private static IFormFile CreateMockFile(long size, string fileName = "test.txt")
    {
        var content = new byte[size];
        var stream = new MemoryStream(content);
        return new FormFile(stream, 0, size, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/plain"
        };
    }

    [Fact]
    public void IsValid_FileSizeWithinRange_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(100, 1000);
        var file = CreateMockFile(500);
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_FileSizeAtMinimum_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(100, 1000);
        var file = CreateMockFile(100);
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_FileSizeAtMaximum_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(100, 1000);
        var file = CreateMockFile(1000);
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_FileSizeBelowMinimum_ReturnsFalse()
    {
        var attribute = new FileSizeAttribute(100, 1000);
        var file = CreateMockFile(50);
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_FileSizeAboveMaximum_ReturnsFalse()
    {
        var attribute = new FileSizeAttribute(100, 1000);
        var file = CreateMockFile(1500);
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_MaxOnlyConstructor_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(1000);
        var file = CreateMockFile(500);
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_MaxOnlyConstructor_ZeroSizeFile_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(1000);
        var file = CreateMockFile(0);
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new FileSizeAttribute(1000);
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonFileType_ReturnsFalse()
    {
        var attribute = new FileSizeAttribute(1000);
        Assert.False(attribute.IsValid("not a file"));
    }

    [Fact]
    public void Constructor_NegativeMinimum_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FileSizeAttribute(-1, 1000));
    }

    [Fact]
    public void Constructor_MaxLessThanMin_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FileSizeAttribute(1000, 100));
    }

    [Fact]
    public void KB_ReturnsCorrectBytes()
    {
        Assert.Equal(1024, FileSizeAttribute.KB(1));
        Assert.Equal(5120, FileSizeAttribute.KB(5));
    }

    [Fact]
    public void MB_ReturnsCorrectBytes()
    {
        Assert.Equal(1024 * 1024, FileSizeAttribute.MB(1));
        Assert.Equal(5 * 1024 * 1024, FileSizeAttribute.MB(5));
    }

    [Fact]
    public void GB_ReturnsCorrectBytes()
    {
        Assert.Equal(1024L * 1024 * 1024, FileSizeAttribute.GB(1));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new FileSizeAttribute(FileSizeAttribute.KB(1), FileSizeAttribute.MB(5));
        var message = attribute.FormatErrorMessage("Document");
        Assert.Contains("Document", message);
        Assert.Contains("1.0 KB", message);
        Assert.Contains("5.0 MB", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new FileSizeAttribute(1000) { ErrorMessage = "{0} is too large" };
        var message = attribute.FormatErrorMessage("File");
        Assert.Equal("File is too large", message);
    }
}
