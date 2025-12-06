namespace DotValidate.Attributes.Booleans;

/// <summary>
/// Specifies that a boolean property must be true if another property has one of the specified values.
/// </summary>
public sealed class AcceptedIfAttribute : ConditionalValidationAttribute
{
    protected override string DefaultErrorMessage => "{0} must be accepted.";

    /// <summary>
    /// Initializes a new instance specifying when the property must be true.
    /// </summary>
    /// <param name="dependentProperty">The name of the property to check.</param>
    /// <param name="values">The values that require this property to be true.</param>
    public AcceptedIfAttribute(string dependentProperty, params object[] values)
        : base(dependentProperty, values)
    {
    }

    public override bool IsValid(object? value, object instance)
    {
        if (!IsConditionMet(instance))
        {
            return true; // Condition not met, validation passes
        }

        return value is true;
    }
}
