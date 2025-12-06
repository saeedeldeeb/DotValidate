namespace DotValidate.Attributes.Utilities;

/// <summary>
/// Specifies that a property must have the same value as another property.
/// Useful for confirmation fields (e.g., confirm password, confirm email).
/// </summary>
public sealed class SameAttribute : ConditionalValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must match {1}.";

    /// <summary>
    /// Initializes a new instance specifying the property to compare against.
    /// </summary>
    /// <param name="otherProperty">The name of the property that must have the same value.</param>
    public SameAttribute(string otherProperty) : base(otherProperty)
    {
    }

    public override bool IsValid(object? value, object instance)
    {
        var property = instance.GetType().GetProperty(DependentProperty);
        if (property is null)
        {
            return true; // Property not found, skip validation
        }

        var otherValue = property.GetValue(instance);
        return Equals(value, otherValue);
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, DependentProperty);
    }
}
