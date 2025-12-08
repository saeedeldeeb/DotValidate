using DotValidation.Attributes.Dates;
using Xunit;

namespace DotValidation.Tests.Attributes.Dates;

public class BeforeOrEqualAttributeTests
{
    [Fact]
    public void IsValid_DateBefore_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateEqual_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateTime(2024, 12, 31)));
    }

    [Fact]
    public void IsValid_DateAfter_ReturnsFalse()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.False(attribute.IsValid(new DateTime(2025, 1, 1)));
    }

    [Fact]
    public void IsValid_Today_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today));
    }

    [Fact]
    public void IsValid_Yesterday_BeforeToday_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void IsValid_Tomorrow_AfterToday_ReturnsFalse()
    {
        var attribute = new BeforeOrEqualAttribute("today");
        Assert.False(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void IsValid_DateTimeOffset_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [Fact]
    public void IsValid_DateOnly_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateOnly(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_StringDate_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid("2024-06-15"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_InvalidDate_ReturnsFalse()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.False(attribute.IsValid("not a date"));
    }

    [Fact]
    public void IsValid_NonDateType_ReturnsFalse()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_EmptyDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new BeforeOrEqualAttribute(""));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new BeforeOrEqualAttribute("2024-12-31");
        var message = attribute.FormatErrorMessage("EndDate");
        Assert.Equal("EndDate must be before or equal to 2024-12-31.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new BeforeOrEqualAttribute("today") { ErrorMessage = "{0} cannot be after {1}" };
        var message = attribute.FormatErrorMessage("BirthDate");
        Assert.Equal("BirthDate cannot be after today", message);
    }
}
