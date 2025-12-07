namespace DotValidate.Attributes.Numbers;

/// <summary>
/// Specifies that a numeric property must have a specific number of decimal places.
/// </summary>
public sealed class DecimalPlacesAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the minimum number of decimal places required.
    /// </summary>
    public int Minimum { get; }

    /// <summary>
    /// Gets the maximum number of decimal places allowed.
    /// </summary>
    public int Maximum { get; }

    protected override string DefaultErrorMessage =>
        Minimum == Maximum
            ? "{0} must have exactly {1} decimal places."
            : "{0} must have between {1} and {2} decimal places.";

    /// <summary>
    /// Initializes a new instance requiring an exact number of decimal places.
    /// </summary>
    /// <param name="places">The exact number of decimal places required.</param>
    public DecimalPlacesAttribute(int places) : this(places, places)
    {
    }

    /// <summary>
    /// Initializes a new instance with minimum and maximum decimal places.
    /// </summary>
    /// <param name="minimum">The minimum number of decimal places.</param>
    /// <param name="maximum">The maximum number of decimal places.</param>
    public DecimalPlacesAttribute(int minimum, int maximum)
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

        int decimalPlaces = GetDecimalPlaces(value);
        if (decimalPlaces < 0)
        {
            return false; // Not a supported numeric type
        }

        return decimalPlaces >= Minimum && decimalPlaces <= Maximum;
    }

    private static int GetDecimalPlaces(object value)
    {
        string? str = value switch
        {
            decimal d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
            double d => d.ToString(System.Globalization.CultureInfo.InvariantCulture),
            float f => f.ToString(System.Globalization.CultureInfo.InvariantCulture),
            int => "0",
            long => "0",
            short => "0",
            byte => "0",
            string s => s,
            _ => null
        };

        if (str is null)
        {
            return -1;
        }

        // Handle scientific notation
        if (str.Contains('E') || str.Contains('e'))
        {
            if (decimal.TryParse(str, System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out var parsed))
            {
                str = parsed.ToString(System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                return -1;
            }
        }

        var decimalIndex = str.IndexOf('.');
        if (decimalIndex < 0)
        {
            return 0;
        }

        // Count decimal places, excluding trailing zeros for exact comparison
        var decimalPart = str[(decimalIndex + 1)..];
        return decimalPart.Length;
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, Minimum, Maximum);
    }
}
