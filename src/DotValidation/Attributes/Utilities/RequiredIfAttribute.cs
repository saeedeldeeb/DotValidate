namespace DotValidation.Attributes.Utilities;

/// <summary>
/// Specifies that a property is required if another property has one of the specified values.
/// </summary>
public sealed class RequiredIfAttribute : ConditionalValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} is required.";

    /// <summary>
    /// Initializes a new instance specifying when the property is required.
    /// </summary>
    /// <param name="dependentProperty">The name of the property to check.</param>
    /// <param name="values">The values that make this property required.</param>
    public RequiredIfAttribute(string dependentProperty, params object[] values)
        : base(dependentProperty, values)
    {
    }

    public override bool IsValid(object? value, object instance)
    {
        if (!IsConditionMet(instance))
        {
            return true; // Condition not met, validation passes
        }

        if (value is null)
        {
            return false;
        }

        if (value is string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }

        return true;
    }
}
