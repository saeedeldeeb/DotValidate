namespace DotValidation.Attributes.Dates;

/// <summary>
/// Specifies that a date property value must be before or equal to the specified date.
/// Supports keywords: "now", "today", "utcnow", or any valid date string.
/// </summary>
public sealed class BeforeOrEqualAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the date to compare against.
    /// </summary>
    public string CompareDate { get; }

    protected override string DefaultErrorMessage => "{0} must be before or equal to {1}.";

    /// <summary>
    /// Initializes a new instance with the date to compare against.
    /// </summary>
    /// <param name="date">A date string or keyword ("now", "today", "utcnow").</param>
    public BeforeOrEqualAttribute(string date)
    {
        if (string.IsNullOrWhiteSpace(date))
        {
            throw new ArgumentException("Date cannot be null or empty.", nameof(date));
        }

        CompareDate = date;
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true; // Use [Required] to enforce non-null
        }

        var compareDateTime = ResolveDate(CompareDate);
        if (compareDateTime is null)
        {
            return false;
        }

        var valueDateTime = ConvertToDateTime(value);
        if (valueDateTime is null)
        {
            return false;
        }

        return valueDateTime <= compareDateTime;
    }

    private static DateTime? ResolveDate(string date)
    {
        return date.ToLowerInvariant() switch
        {
            "now" => DateTime.Now,
            "today" => DateTime.Today,
            "utcnow" => DateTime.UtcNow,
            _ => DateTime.TryParse(date, out var parsed) ? parsed : null
        };
    }

    private static DateTime? ConvertToDateTime(object value)
    {
        return value switch
        {
            DateTime dt => dt,
            DateTimeOffset dto => dto.DateTime,
            DateOnly d => d.ToDateTime(TimeOnly.MinValue),
            string s when DateTime.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, CompareDate);
    }
}
