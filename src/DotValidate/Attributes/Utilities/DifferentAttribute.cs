namespace DotValidate.Attributes.Utilities;

/// <summary>
/// Specifies that a property must have a different value than another property.
/// </summary>
public sealed class DifferentAttribute : ConditionalValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be different from {1}.";

    /// <summary>
    /// Initializes a new instance specifying the property to compare against.
    /// </summary>
    /// <param name="otherProperty">The name of the property that must have a different value.</param>
    public DifferentAttribute(string otherProperty) : base(otherProperty)
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
        return !Equals(value, otherValue);
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, DependentProperty);
    }
}
