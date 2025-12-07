using DotValidate.Attributes.Files;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace DotValidate.Tests.Attributes.Files;

public class ImageAttributeTests
{
    private static IFormFile CreateMockFile(string contentType, string fileName = "image.jpg")
    {
        var content = new byte[100];
        var stream = new MemoryStream(content);
        return new FormFile(stream, 0, content.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };
    }

    [Theory]
    [InlineData("image/jpeg")]
    [InlineData("image/png")]
    [InlineData("image/gif")]
    [InlineData("image/bmp")]
    [InlineData("image/webp")]
    [InlineData("image/svg+xml")]
    [InlineData("image/x-icon")]
    [InlineData("image/tiff")]
    public void IsValid_ImageMimeType_ReturnsTrue(string mimeType)
    {
        var attribute = new ImageAttribute();
        var file = CreateMockFile(mimeType);
        Assert.True(attribute.IsValid(file));
    }

    [Theory]
    [InlineData("application/pdf")]
    [InlineData("text/plain")]
    [InlineData("video/mp4")]
    [InlineData("audio/mpeg")]
    [InlineData("application/octet-stream")]
    public void IsValid_NonImageMimeType_ReturnsFalse(string mimeType)
    {
        var attribute = new ImageAttribute();
        var file = CreateMockFile(mimeType);
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_CaseInsensitive_ReturnsTrue()
    {
        var attribute = new ImageAttribute();
        var file = CreateMockFile("IMAGE/JPEG");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_MimeTypeWithParameters_ReturnsTrue()
    {
        var attribute = new ImageAttribute();
        var file = CreateMockFile("image/png; charset=utf-8");
        Assert.True(attribute.IsValid(file));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new ImageAttribute();
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_NonFileType_ReturnsFalse()
    {
        var attribute = new ImageAttribute();
        Assert.False(attribute.IsValid("not a file"));
    }

    [Fact]
    public void IsValid_EmptyContentType_ReturnsFalse()
    {
        var attribute = new ImageAttribute();
        var file = CreateMockFile("");
        Assert.False(attribute.IsValid(file));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new ImageAttribute();
        var message = attribute.FormatErrorMessage("Avatar");
        Assert.Equal("Avatar must be an image file.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new ImageAttribute { ErrorMessage = "{0} is not a valid image" };
        var message = attribute.FormatErrorMessage("Photo");
        Assert.Equal("Photo is not a valid image", message);
    }

    [Fact]
    public void ImageMimeTypes_ContainsExpectedTypes()
    {
        Assert.Contains("image/jpeg", ImageAttribute.ImageMimeTypes);
        Assert.Contains("image/png", ImageAttribute.ImageMimeTypes);
        Assert.Contains("image/gif", ImageAttribute.ImageMimeTypes);
        Assert.Contains("image/webp", ImageAttribute.ImageMimeTypes);
        Assert.Contains("image/svg+xml", ImageAttribute.ImageMimeTypes);
    }
}
