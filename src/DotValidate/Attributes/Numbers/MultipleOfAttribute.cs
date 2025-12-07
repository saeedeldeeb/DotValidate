namespace DotValidate.Attributes.Numbers;

/// <summary>
/// Specifies that a numeric property must be a multiple of the specified value.
/// </summary>
public sealed class MultipleOfAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the value that the property must be a multiple of.
    /// </summary>
    public double Value { get; }

    protected override string DefaultErrorMessage => "{0} must be a multiple of {1}.";

    /// <summary>
    /// Initializes a new instance specifying the value to check against.
    /// </summary>
    /// <param name="value">The value that the property must be a multiple of.</param>
    public MultipleOfAttribute(double value)
    {
        if (value == 0)
        {
            throw new ArgumentException("Value cannot be zero.", nameof(value));
        }

        Value = value;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        double? number = value switch
        {
            int i => i,
            long l => l,
            short s => s,
            byte b => b,
            sbyte sb => sb,
            uint ui => ui,
            ulong ul => ul,
            ushort us => us,
            double d => d,
            float f => f,
            decimal m => (double)m,
            string s when double.TryParse(s, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var parsed) => parsed,
            _ => null
        };

        if (number is null)
        {
            return false; // Not a supported numeric type
        }

        // Use modulo with tolerance for floating point precision
        var remainder = Math.Abs(number.Value % Value);
        var tolerance = Math.Abs(Value) * 1e-10;

        return remainder < tolerance || Math.Abs(remainder - Math.Abs(Value)) < tolerance;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, Value);
    }
}
