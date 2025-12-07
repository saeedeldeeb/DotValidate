using System.Collections;

namespace DotValidate.Attributes.Collections;

/// <summary>
/// Specifies that a collection property must have a size between the given minimum and maximum values (inclusive).
/// </summary>
public sealed class BetweenAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the minimum size (inclusive).
    /// </summary>
    public int Minimum { get; }

    /// <summary>
    /// Gets the maximum size (inclusive).
    /// </summary>
    public int Maximum { get; }

    protected override string DefaultErrorMessage => "{0} must have between {1} and {2} items.";

    /// <summary>
    /// Initializes a new instance with the minimum and maximum size constraints.
    /// </summary>
    /// <param name="minimum">The minimum number of items (inclusive).</param>
    /// <param name="maximum">The maximum number of items (inclusive).</param>
    public BetweenAttribute(int minimum, int maximum)
    {
        if (minimum < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be non-negative.");
        }

        if (maximum < minimum)
        {
            throw new ArgumentOutOfRangeException(nameof(maximum), "Maximum must be greater than or equal to minimum.");
        }

        Minimum = minimum;
        Maximum = maximum;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        int count;

        if (value is ICollection collection)
        {
            count = collection.Count;
        }
        else if (value is IEnumerable enumerable)
        {
            count = enumerable.Cast<object>().Count();
        }
        else
        {
            return false; // Not a collection
        }

        return count >= Minimum && count <= Maximum;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, Minimum, Maximum);
    }
}
