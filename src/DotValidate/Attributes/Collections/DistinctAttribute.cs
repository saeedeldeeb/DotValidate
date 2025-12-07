using System.Collections;

namespace DotValidate.Attributes.Collections;

/// <summary>
/// Specifies that a collection property must contain only unique values (no duplicates).
/// </summary>
public sealed class DistinctAttribute : ValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must contain unique values.";

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        if (value is not IEnumerable enumerable)
        {
            return false;
        }

        var seen = new HashSet<object?>();

        foreach (var item in enumerable)
        {
            if (!seen.Add(item))
            {
                return false; // Duplicate found
            }
        }

        return true;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName);
    }
}
