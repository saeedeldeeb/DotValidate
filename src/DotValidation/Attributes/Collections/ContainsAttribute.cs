using System.Collections;

namespace DotValidation.Attributes.Collections;

/// <summary>
/// Specifies that a collection property must contain all of the specified values.
/// </summary>
public sealed class ContainsAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the expected values that must be present in the collection.
    /// </summary>
    public object[] ExpectedValues { get; }

    protected override string DefaultErrorMessage =>
        ExpectedValues.Length == 1
            ? "{0} must contain '{1}'."
            : "{0} must contain all of: {1}.";

    /// <summary>
    /// Initializes a new instance with the values that must be present in the collection.
    /// </summary>
    /// <param name="values">The values that must all be present in the collection.</param>
    public ContainsAttribute(params object[] values)
    {
        if (values is null || values.Length == 0)
        {
            throw new ArgumentException("At least one value must be specified.", nameof(values));
        }

        ExpectedValues = values;
    }

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

        var collection = enumerable.Cast<object>().ToList();

        // Check that ALL expected values are present
        return ExpectedValues.All(expected =>
            collection.Any(item => Equals(item, expected)));
    }

    public override string FormatErrorMessage(string propertyName)
    {
        var valuesString = ExpectedValues.Length == 1
            ? ExpectedValues[0]?.ToString() ?? "null"
            : string.Join(", ", ExpectedValues.Select(v => $"'{v}'"));

        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, valuesString);
    }
}
