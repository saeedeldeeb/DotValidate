namespace DotValidate.Attributes.Numbers;

/// <summary>
/// Specifies that a numeric property value must be within a specified range.
/// </summary>
public sealed class RangeAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the minimum value.
    /// </summary>
    public object Minimum { get; }

    /// <summary>
    /// Gets the maximum value.
    /// </summary>
    public object Maximum { get; }

    /// <summary>
    /// Gets the type of the range.
    /// </summary>
    public Type OperandType { get; }

    protected override string DefaultErrorMessage => "{0} must be between {1} and {2}.";

    /// <summary>
    /// Initializes a new instance with integer bounds.
    /// </summary>
    public RangeAttribute(int minimum, int maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        OperandType = typeof(int);
    }

    /// <summary>
    /// Initializes a new instance with double bounds.
    /// </summary>
    public RangeAttribute(double minimum, double maximum)
    {
        Minimum = minimum;
        Maximum = maximum;
        OperandType = typeof(double);
    }

    /// <summary>
    /// Initializes a new instance with typed bounds.
    /// </summary>
    public RangeAttribute(Type type, string minimum, string maximum)
    {
        OperandType = type;
        Minimum = Convert.ChangeType(minimum, type)!;
        Maximum = Convert.ChangeType(maximum, type)!;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        try
        {
            var convertedValue = Convert.ChangeType(value, OperandType);
            var comparable = convertedValue as IComparable;

            if (comparable is null)
            {
                return false;
            }

            return comparable.CompareTo(Minimum) >= 0 && comparable.CompareTo(Maximum) <= 0;
        }
        catch
        {
            return false;
        }
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, Minimum, Maximum);
    }
}
