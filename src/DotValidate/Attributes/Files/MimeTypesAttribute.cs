using Microsoft.AspNetCore.Http;

namespace DotValidate.Attributes.Files;

/// <summary>
/// Specifies that a file property must have one of the allowed MIME types.
/// </summary>
public sealed class MimeTypesAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the allowed MIME types.
    /// </summary>
    public string[] AllowedMimeTypes { get; }

    protected override string DefaultErrorMessage => "{0} must have one of the following types: {1}.";

    /// <summary>
    /// Initializes a new instance with the allowed MIME types.
    /// </summary>
    /// <param name="mimeTypes">The allowed MIME types (e.g., "application/pdf", "image/png").</param>
    public MimeTypesAttribute(params string[] mimeTypes)
    {
        if (mimeTypes is null || mimeTypes.Length == 0)
        {
            throw new ArgumentException("At least one MIME type must be specified.", nameof(mimeTypes));
        }

        AllowedMimeTypes = mimeTypes.Select(m => m.ToLowerInvariant()).ToArray();
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value switch
        {
            IFormFile file => IsValidMimeType(file.ContentType),
            IFormFileCollection files => files.All(f => IsValidMimeType(f.ContentType)),
            _ => false
        };
    }

    private bool IsValidMimeType(string? contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
        {
            return false;
        }

        var mimeType = contentType.ToLowerInvariant();

        // Handle MIME types with parameters (e.g., "text/plain; charset=utf-8")
        var semicolonIndex = mimeType.IndexOf(';');
        if (semicolonIndex > 0)
        {
            mimeType = mimeType[..semicolonIndex].Trim();
        }

        return AllowedMimeTypes.Contains(mimeType);
    }

    public override string FormatErrorMessage(string propertyName)
    {
        var mimeTypeList = string.Join(", ", AllowedMimeTypes);
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, mimeTypeList);
    }
}
