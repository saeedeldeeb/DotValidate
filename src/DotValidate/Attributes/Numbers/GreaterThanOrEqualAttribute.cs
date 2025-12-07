namespace DotValidate.Attributes.Numbers;

/// <summary>
/// Specifies that a property must be greater than or equal to another property.
/// Works with any type that implements IComparable (numbers, dates, etc.).
/// </summary>
public sealed class GreaterThanOrEqualAttribute : ConditionalValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be greater than or equal to {1}.";

    /// <summary>
    /// Initializes a new instance specifying the property to compare against.
    /// </summary>
    /// <param name="otherProperty">The name of the property that this value must be greater than or equal to.</param>
    public GreaterThanOrEqualAttribute(string otherProperty) : base(otherProperty)
    {
    }

    public override bool IsValid(object? value, object instance)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        var property = instance.GetType().GetProperty(DependentProperty);
        if (property is null)
        {
            return true; // Property not found, skip validation
        }

        var otherValue = property.GetValue(instance);
        if (otherValue is null)
        {
            return true; // Other value is null, skip comparison
        }

        if (value is IComparable comparable)
        {
            return comparable.CompareTo(otherValue) >= 0;
        }

        return true; // Not comparable, skip validation
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, DependentProperty);
    }
}
