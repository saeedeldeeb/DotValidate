using DotValidate.Attributes.Dates;
using Xunit;

namespace DotValidate.Tests.Attributes.Dates;

public class DateEqualsAttributeTests
{
    [Fact]
    public void IsValid_DateEquals_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateNotEquals_ReturnsFalse()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.False(attribute.IsValid(new DateTime(2024, 6, 16)));
    }

    [Fact]
    public void IsValid_DateOnlyMode_IgnoresTime_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15") { DateOnly = true };
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15, 14, 30, 0)));
    }

    [Fact]
    public void IsValid_ExactMode_DifferentTime_ReturnsFalse()
    {
        var attribute = new DateEqualsAttribute("2024-06-15") { DateOnly = false };
        Assert.False(attribute.IsValid(new DateTime(2024, 6, 15, 14, 30, 0)));
    }

    [Fact]
    public void IsValid_ExactMode_SameDateTime_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15 14:30:00") { DateOnly = false };
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15, 14, 30, 0)));
    }

    [Fact]
    public void IsValid_Today_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today));
    }

    [Fact]
    public void IsValid_Today_WithTime_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("today") { DateOnly = true };
        Assert.True(attribute.IsValid(DateTime.Now)); // Same date, different time
    }

    [Fact]
    public void IsValid_Tomorrow_ReturnsFalse()
    {
        var attribute = new DateEqualsAttribute("today");
        Assert.False(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void IsValid_DateTimeOffset_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.IsValid(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [Fact]
    public void IsValid_DateOnly_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.IsValid(new DateOnly(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_StringDate_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.IsValid("2024-06-15"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_InvalidDate_ReturnsFalse()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.False(attribute.IsValid("not a date"));
    }

    [Fact]
    public void IsValid_NonDateType_ReturnsFalse()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_EmptyDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new DateEqualsAttribute(""));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        var message = attribute.FormatErrorMessage("EventDate");
        Assert.Equal("EventDate must equal 2024-06-15.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new DateEqualsAttribute("today") { ErrorMessage = "{0} must be {1}" };
        var message = attribute.FormatErrorMessage("Date");
        Assert.Equal("Date must be today", message);
    }

    [Fact]
    public void DateOnly_DefaultsToTrue()
    {
        var attribute = new DateEqualsAttribute("2024-06-15");
        Assert.True(attribute.DateOnly);
    }
}
