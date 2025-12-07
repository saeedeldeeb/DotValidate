using DotValidate.Attributes.Dates;
using Xunit;

namespace DotValidate.Tests.Attributes.Dates;

public class AfterOrEqualAttributeTests
{
    [Fact]
    public void IsValid_DateAfter_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateEqual_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateTime(2024, 1, 1)));
    }

    [Fact]
    public void IsValid_DateBefore_ReturnsFalse()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.False(attribute.IsValid(new DateTime(2023, 12, 31)));
    }

    [Fact]
    public void IsValid_Today_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today));
    }

    [Fact]
    public void IsValid_Tomorrow_AfterToday_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("today");
        Assert.True(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void IsValid_Yesterday_BeforeToday_ReturnsFalse()
    {
        var attribute = new AfterOrEqualAttribute("today");
        Assert.False(attribute.IsValid(DateTime.Today.AddDays(-1)));
    }

    [Fact]
    public void IsValid_DateTimeOffset_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [Fact]
    public void IsValid_DateOnly_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid(new DateOnly(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_StringDate_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid("2024-06-15"));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_InvalidDate_ReturnsFalse()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.False(attribute.IsValid("not a date"));
    }

    [Fact]
    public void IsValid_NonDateType_ReturnsFalse()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        Assert.False(attribute.IsValid(123));
    }

    [Fact]
    public void Constructor_EmptyDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new AfterOrEqualAttribute(""));
    }

    [Fact]
    public void FormatErrorMessage_ReturnsCorrectMessage()
    {
        var attribute = new AfterOrEqualAttribute("2024-01-01");
        var message = attribute.FormatErrorMessage("StartDate");
        Assert.Equal("StartDate must be after or equal to 2024-01-01.", message);
    }

    [Fact]
    public void FormatErrorMessage_CustomMessage_ReturnsCustomMessage()
    {
        var attribute = new AfterOrEqualAttribute("today") { ErrorMessage = "{0} must be {1} or later" };
        var message = attribute.FormatErrorMessage("EventDate");
        Assert.Equal("EventDate must be today or later", message);
    }
}
