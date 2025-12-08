namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must end with one of the given values.
/// </summary>
public sealed class EndsWithAttribute : ValidationAttribute
{
    /// <summary>
    /// The values that the string must end with (any one of them).
    /// </summary>
    public string[] Values { get; }

    /// <summary>
    /// Gets or sets whether the comparison is case-insensitive. Default is false (case-sensitive).
    /// </summary>
    public bool IgnoreCase { get; set; }

    protected override string DefaultErrorMessage => "{0} must end with one of the following: {1}.";

    /// <summary>
    /// Initializes a new instance specifying values the string must end with.
    /// </summary>
    /// <param name="values">The values that the string must end with (any one of them).</param>
    public EndsWithAttribute(params string[] values)
    {
        if (values is null || values.Length == 0)
        {
            throw new ArgumentException("At least one value must be specified.", nameof(values));
        }

        Values = values;
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

        if (string.IsNullOrEmpty(str))
        {
            return true; // Use [Required] to enforce non-empty
        }

        var comparison = IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

        foreach (var v in Values)
        {
            if (str.EndsWith(v, comparison))
            {
                return true;
            }
        }

        return false;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, string.Join(", ", Values));
    }
}
