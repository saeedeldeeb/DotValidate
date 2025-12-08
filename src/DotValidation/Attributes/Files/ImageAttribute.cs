using Microsoft.AspNetCore.Http;

namespace DotValidation.Attributes.Files;

/// <summary>
/// Specifies that a file property must be an image file.
/// Accepts common image MIME types: jpeg, png, gif, bmp, webp, svg, ico, tiff.
/// </summary>
public sealed class ImageAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the allowed image MIME types.
    /// </summary>
    public static readonly string[] ImageMimeTypes =
    [
        "image/jpeg",
        "image/png",
        "image/gif",
        "image/bmp",
        "image/webp",
        "image/svg+xml",
        "image/x-icon",
        "image/tiff"
    ];

    protected override string DefaultErrorMessage => "{0} must be an image file.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value switch
        {
            IFormFile file => IsValidImage(file.ContentType),
            IFormFileCollection files => files.All(f => IsValidImage(f.ContentType)),
            _ => false
        };
    }

    private static bool IsValidImage(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return false;
        }

        var mimeType = contentType.ToLowerInvariant();

        // Handle MIME types with parameters
        var semicolonIndex = mimeType.IndexOf(';');
        if (semicolonIndex > 0)
        {
            mimeType = mimeType[..semicolonIndex].Trim();
        }

        return ImageMimeTypes.Contains(mimeType);
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName);
    }
}
