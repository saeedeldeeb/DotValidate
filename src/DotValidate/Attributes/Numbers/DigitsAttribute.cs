namespace DotValidate.Attributes.Numbers;

/// <summary>
/// Specifies that an integer property must have a specific number of digits.
/// </summary>
public sealed class DigitsAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the minimum number of digits required.
    /// </summary>
    public int Minimum { get; }

    /// <summary>
    /// Gets the maximum number of digits allowed.
    /// </summary>
    public int Maximum { get; }

    protected override string DefaultErrorMessage =>
        Minimum == Maximum
            ? "{0} must have exactly {1} digits."
            : "{0} must have between {1} and {2} digits.";

    /// <summary>
    /// Initializes a new instance requiring an exact number of digits.
    /// </summary>
    /// <param name="digits">The exact number of digits required.</param>
    public DigitsAttribute(int digits) : this(digits, digits)
    {
    }

    /// <summary>
    /// Initializes a new instance with minimum and maximum digit count.
    /// </summary>
    /// <param name="minimum">The minimum number of digits.</param>
    /// <param name="maximum">The maximum number of digits.</param>
    public DigitsAttribute(int minimum, int maximum)
    {
        if (minimum < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(minimum), "Minimum must be at least 1.");
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

        long? number = value switch
        {
            int i => Math.Abs(i),
            long l => Math.Abs(l),
            short s => Math.Abs(s),
            byte b => b,
            sbyte sb => Math.Abs(sb),
            uint ui => ui,
            ulong ul => (long)ul,
            ushort us => us,
            string s when long.TryParse(s.TrimStart('-'), out var parsed) => parsed,
            _ => null
        };

        if (number is null)
        {
            return false; // Not a supported integer type
        }

        int digitCount = number == 0 ? 1 : (int)Math.Floor(Math.Log10(number.Value) + 1);

        return digitCount >= Minimum && digitCount <= Maximum;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, Minimum, Maximum);
    }
}
