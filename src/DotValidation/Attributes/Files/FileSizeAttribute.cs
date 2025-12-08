using Microsoft.AspNetCore.Http;

namespace DotValidation.Attributes.Files;

/// <summary>
/// Specifies that a file property must have a size within the specified range.
/// </summary>
public sealed class FileSizeAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the minimum file size in bytes.
    /// </summary>
    public long MinBytes { get; }

    /// <summary>
    /// Gets the maximum file size in bytes.
    /// </summary>
    public long MaxBytes { get; }

    protected override string DefaultErrorMessage => "{0} must be between {1} and {2}.";

    /// <summary>
    /// Initializes a new instance with maximum file size only.
    /// </summary>
    /// <param name="maxBytes">The maximum file size in bytes.</param>
    public FileSizeAttribute(long maxBytes) : this(0, maxBytes)
    {
    }

    /// <summary>
    /// Initializes a new instance with minimum and maximum file size.
    /// </summary>
    /// <param name="minBytes">The minimum file size in bytes.</param>
    /// <param name="maxBytes">The maximum file size in bytes.</param>
    public FileSizeAttribute(long minBytes, long maxBytes)
    {
        if (minBytes < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minBytes), "Minimum size must be non-negative.");
        }

        if (maxBytes < minBytes)
        {
            throw new ArgumentOutOfRangeException(nameof(maxBytes), "Maximum size must be greater than or equal to minimum size.");
        }

        MinBytes = minBytes;
        MaxBytes = maxBytes;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        long? fileSize = value switch
        {
            IFormFile file => file.Length,
            IFormFileCollection files => files.Sum(f => f.Length),
            _ => null
        };

        if (fileSize is null)
        {
            return false; // Not a file type
        }

        return fileSize >= MinBytes && fileSize <= MaxBytes;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, FormatSize(MinBytes), FormatSize(MaxBytes));
    }

    private static string FormatSize(long bytes)
    {
        if (bytes >= 1024 * 1024 * 1024)
            return $"{bytes / (1024 * 1024 * 1024.0):F1} GB";
        if (bytes >= 1024 * 1024)
            return $"{bytes / (1024 * 1024.0):F1} MB";
        if (bytes >= 1024)
            return $"{bytes / 1024.0:F1} KB";
        return $"{bytes} bytes";
    }

    /// <summary>
    /// Helper to convert kilobytes to bytes.
    /// </summary>
    public static long KB(long kilobytes) => kilobytes * 1024;

    /// <summary>
    /// Helper to convert megabytes to bytes.
    /// </summary>
    public static long MB(long megabytes) => megabytes * 1024 * 1024;

    /// <summary>
    /// Helper to convert gigabytes to bytes.
    /// </summary>
    public static long GB(long gigabytes) => gigabytes * 1024 * 1024 * 1024;
}
