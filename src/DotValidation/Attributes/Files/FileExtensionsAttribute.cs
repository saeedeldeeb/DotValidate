using Microsoft.AspNetCore.Http;

namespace DotValidation.Attributes.Files;

/// <summary>
/// Specifies that a file property must have one of the allowed extensions.
/// </summary>
public sealed class FileExtensionsAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the allowed file extensions.
    /// </summary>
    public string[] AllowedExtensions { get; }

    protected override string DefaultErrorMessage => "{0} must have one of the following extensions: {1}.";

    /// <summary>
    /// Initializes a new instance with the allowed file extensions.
    /// </summary>
    /// <param name="extensions">The allowed extensions (e.g., ".pdf", ".doc", ".docx").</param>
    public FileExtensionsAttribute(params string[] extensions)
    {
        if (extensions is null || extensions.Length == 0)
        {
            throw new ArgumentException("At least one extension must be specified.", nameof(extensions));
        }

        // Normalize extensions to lowercase with leading dot
        AllowedExtensions = extensions
            .Select(e => e.StartsWith('.') ? e.ToLowerInvariant() : $".{e.ToLowerInvariant()}")
            .ToArray();
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        return value switch
        {
            IFormFile file => IsValidExtension(file.FileName),
            IFormFileCollection files => files.All(f => IsValidExtension(f.FileName)),
            _ => false
        };
    }

    private bool IsValidExtension(string? fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }

    public override string FormatErrorMessage(string propertyName)
    {
        var extensionList = string.Join(", ", AllowedExtensions);
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, extensionList);
    }
}
