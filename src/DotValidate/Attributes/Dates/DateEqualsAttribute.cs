namespace DotValidate.Attributes.Dates;

/// <summary>
/// Specifies that a date property value must be equal to the specified date.
/// Supports keywords: "now", "today", "utcnow", or any valid date string.
/// </summary>
public sealed class DateEqualsAttribute : ValidationAttribute
{
    /// <summary>
    /// Gets the date to compare against.
    /// </summary>
    public string CompareDate { get; }

    /// <summary>
    /// Gets or sets whether to compare only the date part (ignoring time).
    /// Default is true.
    /// </summary>
    public bool DateOnly { get; set; } = true;

    protected override string DefaultErrorMessage => "{0} must equal {1}.";

    /// <summary>
    /// Initializes a new instance with the date to compare against.
    /// </summary>
    /// <param name="date">A date string or keyword ("now", "today", "utcnow").</param>
    public DateEqualsAttribute(string date)
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

        if (DateOnly)
        {
            return valueDateTime.Value.Date == compareDateTime.Value.Date;
        }

        return valueDateTime == compareDateTime;
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
            System.DateOnly d => d.ToDateTime(TimeOnly.MinValue),
            string s when DateTime.TryParse(s, out var parsed) => parsed,
            _ => null
        };
    }

    public override string FormatErrorMessage(string propertyName)
    {
        return string.Format(ErrorMessage ?? DefaultErrorMessage, propertyName, CompareDate);
    }
}
