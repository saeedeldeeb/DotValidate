using DotValidation.Attributes.Dates;
using Xunit;

namespace DotValidation.Tests.Attributes.Dates;

public class BeforeAttributeTests
{
    [Fact]
    public void IsValid_DateTimeBeforeCompareDate_ReturnsTrue()
    {
        var attribute = new BeforeAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeAfterCompareDate_ReturnsFalse()
    {
        var attribute = new BeforeAttribute("2024-01-01");
        Assert.False(attribute.IsValid(new DateTime(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeEqualToCompareDate_ReturnsFalse()
    {
        var attribute = new BeforeAttribute("2024-01-01");
        Assert.False(attribute.IsValid(new DateTime(2024, 1, 1)));
    }

    [Fact]
    public void IsValid_Now_PastDateTime_ReturnsTrue()
    {
        var attribute = new BeforeAttribute("now");
        Assert.True(attribute.IsValid(DateTime.Now.AddHours(-1)));
    }

    [Fact]
    public void IsValid_Today_FutureDate_ReturnsFalse()
    {
        var attribute = new BeforeAttribute("today");
        Assert.False(attribute.IsValid(DateTime.Today.AddDays(1)));
    }

    [Fact]
    public void IsValid_StringDateValue_ParsesAndCompares()
    {
        var attribute = new BeforeAttribute("2024-12-31");
        Assert.True(attribute.IsValid("2024-06-15"));
        Assert.False(attribute.IsValid("2025-01-15"));
    }

    [Fact]
    public void IsValid_DateOnly_Works()
    {
        var attribute = new BeforeAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateOnly(2024, 6, 15)));
    }

    [Fact]
    public void IsValid_DateTimeOffset_Works()
    {
        var attribute = new BeforeAttribute("2024-12-31");
        Assert.True(attribute.IsValid(new DateTimeOffset(2024, 6, 15, 0, 0, 0, TimeSpan.Zero)));
    }

    [Fact]
    public void IsValid_Null_ReturnsTrue()
    {
        var attribute = new BeforeAttribute("2024-01-01");
        Assert.True(attribute.IsValid(null));
    }

    [Fact]
    public void IsValid_InvalidCompareDate_ReturnsFalse()
    {
        var attribute = new BeforeAttribute("not-a-date");
        Assert.False(attribute.IsValid(DateTime.Now));
    }

    [Fact]
    public void IsValid_InvalidValueType_ReturnsFalse()
    {
        var attribute = new BeforeAttribute("2024-01-01");
        Assert.False(attribute.IsValid(12345));
    }

    [Fact]
    public void Constructor_EmptyDate_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new BeforeAttribute(""));
    }

    [Fact]
    public void FormatErrorMessage_FormatsCorrectly()
    {
        var attribute = new BeforeAttribute("2024-12-31");
        var message = attribute.FormatErrorMessage("EndDate");
        Assert.Contains("EndDate", message);
        Assert.Contains("2024-12-31", message);
    }
}
