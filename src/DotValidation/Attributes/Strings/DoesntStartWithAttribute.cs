namespace DotValidation.Attributes.Strings;

/// <summary>
/// Specifies that a string property must not start with any of the given values.
/// </summary>
public sealed class DoesntStartWithAttribute : ValidationAttribute
{
    /// <summary>
    /// The values that the string must not start with.
    /// </summary>
    public string[] Values { get; }

    /// <summary>
    /// Gets or sets whether the comparison is case-insensitive. Default is false (case-sensitive).
    /// </summary>
    public bool IgnoreCase { get; set; }

    protected override string DefaultErrorMessage => "{0} must not start with any of the following: {1}.";

    /// <summary>
    /// Initializes a new instance specifying values the string must not start with.
    /// </summary>
    /// <param name="values">The values that the string must not start with.</param>
    public DoesntStartWithAttribute(params string[] values)
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
            if (str.StartsWith(v, comparison))
            {
                return false;
            }
        }

        return true;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, string.Join(", ", Values));
    }
}
