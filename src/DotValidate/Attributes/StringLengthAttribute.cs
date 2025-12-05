namespace DotValidate.Attributes;

/// <summary>
/// Specifies the minimum and maximum length of characters allowed in a string property.
/// </summary>
public sealed class StringLengthAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the maximum length.
    /// </summary>
    public int MaximumLength { get; }

    /// <summary>
    /// Gets or sets the minimum length.
    /// </summary>
    public int MinimumLength { get; set; }

    protected override string DefaultErrorMessage =>
        MinimumLength > 0
            ? "{0} must be between {1} and {2} characters."
            : "{0} must not exceed {2} characters.";

    /// <summary>
    /// Initializes a new instance with the maximum length.
    /// </summary>
    public StringLengthAttribute(int maximumLength)
    {
        if (maximumLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumLength), "Maximum length must be non-negative.");
        }

        MaximumLength = maximumLength;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        if (value is not string str)
        {
            return false;
        }

        var length = str.Length;
        return length >= MinimumLength && length <= MaximumLength;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, MinimumLength, MaximumLength);
    }
}
