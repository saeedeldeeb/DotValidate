using DotValidate.Attributes.Dates;
using Xunit;

namespace DotValidate.Tests.Attributes.Dates;

public class AfterAttributeTests
{
    [Fact]
    public void IsValid_DateTimeAfterCompareDate_ReturnsTrue()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeBeforeCompareDate_ReturnsFalse()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.False(attribute.IsValid(new DateTime(2023, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeEqualToCompareDate_ReturnsFalse()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.False(attribute.IsValid(new DateTime(2024, 1, 1)));
    }

    [Fact]
    public void IsValid_Today_FutureDate_ReturnsTrue()
    {
        var attribute = new AfterAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void IsValid_Today_PastDate_ReturnsFalse()
    {
        var attribute = new AfterAttribute("today");
        Assert.False(attribute.IsValid(DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void IsValid_Now_FutureDateTime_ReturnsTrue()
    {
        var attribute = new AfterAttribute("now");
        Assert.True(attribute.IsValid(DateTime.Now.AddHours(1)));
    }

    [Fact]
    public void IsValid_StringDateValue_ParsesAndCompares()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.True(attribute.IsValid("2024-06-15"));
        Assert.False(attribute.IsValid("2023-06-15"));
    }

    [Fact]
    public void IsValid_DateOnly_Works()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateOnly(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeOffset_Works()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_InvalidCompareDate_ReturnsFalse()
    {
        var attribute = new AfterAttribute("not-a-date");
        Assert.False(attribute.IsValid(DateTime.Now));
    }

    [Fact]
    public void IsValid_InvalidValueType_ReturnsFalse()
    {
        var attribute = new AfterAttribute("2024-01-01");
        Assert.False(attribute.IsValid(12345));
    }

    [Fact]
    public void Constructor_EmptyDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AfterAttribute(""));
    }

    [Fact]
    public void FormatErrorMessage_FormatsCorrectly()
    {
        var attribute = new AfterAttribute("2024-01-01");
        var message = attribute.FormatErrorMessage("StartDate");
        Assert.Contains("StartDate", message);
        Assert.Contains("2024-01-01", message);
    }
}
