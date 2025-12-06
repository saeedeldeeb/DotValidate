using DotValidate.Attributes;
using DotValidate.Attributes.Collections;
using DotValidate.Attributes.Dates;
using DotValidate.Attributes.Numbers;
using DotValidate.Attributes.Strings;
using Xunit;

namespace DotValidate.Tests;

public class AttributeTests
{
    public class RequiredAttributeTests
    {
        private readonly RequiredAttribute _attribute = new();

        [Fact]
        public void IsValid_Null_ReturnsFalse()
        {
            Assert.False(_attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_EmptyString_ReturnsFalse()
        {
            Assert.False(_attribute.IsValid(""));
        }

        [Fact]
        public void IsValid_WhitespaceString_ReturnsFalse()
        {
            Assert.False(_attribute.IsValid("   "));
        }

        [Fact]
        public void IsValid_ValidString_ReturnsTrue()
        {
            Assert.True(_attribute.IsValid("hello"));
        }

        [Fact]
        public void IsValid_AllowEmptyStrings_EmptyString_ReturnsTrue()
        {
            var attribute = new RequiredAttribute { AllowEmptyStrings = true };
            Assert.True(attribute.IsValid(""));
        }
    }

    public class EmailAttributeTests
    {
        private readonly EmailAttribute _attribute = new();

        [Theory]
        [InlineData("test@example.com")]
        [InlineData("user.name@domain.org")]
        [InlineData("user+tag@example.co.uk")]
        public void IsValid_ValidEmails_ReturnsTrue(string email)
        {
            Assert.True(_attribute.IsValid(email));
        }

        [Theory]
        [InlineData("invalid")]
        [InlineData("@example.com")]
        [InlineData("test@")]
        [InlineData("test@.com")]
        public void IsValid_InvalidEmails_ReturnsFalse(string email)
        {
            Assert.False(_attribute.IsValid(email));
        }

        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            Assert.True(_attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_EmptyString_ReturnsTrue()
        {
            Assert.True(_attribute.IsValid(""));
        }
    }

    public class RangeAttributeTests
    {
        [Fact]
        public void IsValid_InRange_ReturnsTrue()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.True(attribute.IsValid(5));
        }

        [Fact]
        public void IsValid_AtMinimum_ReturnsTrue()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.True(attribute.IsValid(1));
        }

        [Fact]
        public void IsValid_AtMaximum_ReturnsTrue()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.True(attribute.IsValid(10));
        }

        [Fact]
        public void IsValid_BelowMinimum_ReturnsFalse()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.False(attribute.IsValid(0));
        }

        [Fact]
        public void IsValid_AboveMaximum_ReturnsFalse()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.False(attribute.IsValid(11));
        }

        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            var attribute = new RangeAttribute(1, 10);
            Assert.True(attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_DoubleRange_Works()
        {
            var attribute = new RangeAttribute(1.5, 10.5);
            Assert.True(attribute.IsValid(5.0));
            Assert.False(attribute.IsValid(1.0));
        }
    }

    public class StringLengthAttributeTests
    {
        [Fact]
        public void IsValid_WithinLength_ReturnsTrue()
        {
            var attribute = new StringLengthAttribute(10);
            Assert.True(attribute.IsValid("hello"));
        }

        [Fact]
        public void IsValid_ExceedsLength_ReturnsFalse()
        {
            var attribute = new StringLengthAttribute(5);
            Assert.False(attribute.IsValid("hello world"));
        }

        [Fact]
        public void IsValid_WithMinimum_TooShort_ReturnsFalse()
        {
            var attribute = new StringLengthAttribute(10) { MinimumLength = 3 };
            Assert.False(attribute.IsValid("ab"));
        }

        [Fact]
        public void IsValid_WithMinimum_Valid_ReturnsTrue()
        {
            var attribute = new StringLengthAttribute(10) { MinimumLength = 3 };
            Assert.True(attribute.IsValid("abc"));
        }

        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            var attribute = new StringLengthAttribute(10);
            Assert.True(attribute.IsValid(null));
        }
    }

    public class RegexAttributeTests
    {
        [Fact]
        public void IsValid_MatchingPattern_ReturnsTrue()
        {
            var attribute = new RegexAttribute(@"^\d{3}-\d{4}$");
            Assert.True(attribute.IsValid("123-4567"));
        }

        [Fact]
        public void IsValid_NonMatchingPattern_ReturnsFalse()
        {
            var attribute = new RegexAttribute(@"^\d{3}-\d{4}$");
            Assert.False(attribute.IsValid("abc-defg"));
        }

        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            var attribute = new RegexAttribute(@"^\d+$");
            Assert.True(attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_EmptyString_ReturnsTrue()
        {
            var attribute = new RegexAttribute(@"^\d+$");
            Assert.True(attribute.IsValid(""));
        }
    }

    public class ContainsAttributeTests
    {
        [Fact]
        public void IsValid_SingleValue_Present_ReturnsTrue()
        {
            var attribute = new ContainsAttribute("admin");
            Assert.True(attribute.IsValid(new[] { "admin", "user" }));
        }

        [Fact]
        public void IsValid_SingleValue_Missing_ReturnsFalse()
        {
            var attribute = new ContainsAttribute("admin");
            Assert.False(attribute.IsValid(new[] { "user", "guest" }));
        }

        [Fact]
        public void IsValid_MultipleValues_AllPresent_ReturnsTrue()
        {
            var attribute = new ContainsAttribute("a", "b");
            Assert.True(attribute.IsValid(new[] { "a", "b", "c" }));
        }

        [Fact]
        public void IsValid_MultipleValues_PartialPresent_ReturnsFalse()
        {
            var attribute = new ContainsAttribute("a", "b");
            Assert.False(attribute.IsValid(new[] { "a", "c" }));
        }

        [Fact]
        public void IsValid_Null_ReturnsTrue()
        {
            var attribute = new ContainsAttribute("admin");
            Assert.True(attribute.IsValid(null));
        }

        [Fact]
        public void IsValid_EmptyCollection_ReturnsFalse()
        {
            var attribute = new ContainsAttribute("admin");
            Assert.False(attribute.IsValid(Array.Empty<string>()));
        }

        [Fact]
        public void IsValid_NonCollection_ReturnsFalse()
        {
            var attribute = new ContainsAttribute("admin");
            Assert.False(attribute.IsValid("not a collection"));
        }

        [Fact]
        public void IsValid_IntegerArray_AllPresent_ReturnsTrue()
        {
            var attribute = new ContainsAttribute(1, 2);
            Assert.True(attribute.IsValid(new[] { 1, 2, 3 }));
        }

        [Fact]
        public void IsValid_List_Works()
        {
            var attribute = new ContainsAttribute("x", "y");
            Assert.True(attribute.IsValid(new List<string> { "x", "y", "z" }));
        }

        [Fact]
        public void Constructor_NoValues_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new ContainsAttribute());
        }

        [Fact]
        public void FormatErrorMessage_SingleValue_FormatsCorrectly()
        {
            var attribute = new ContainsAttribute("admin");
            var message = attribute.FormatErrorMessage("Roles");
            Assert.Contains("admin", message);
            Assert.Contains("Roles", message);
        }

        [Fact]
        public void FormatErrorMessage_MultipleValues_FormatsCorrectly()
        {
            var attribute = new ContainsAttribute("a", "b", "c");
            var message = attribute.FormatErrorMessage("Items");
            Assert.Contains("Items", message);
            Assert.Contains("'a'", message);
            Assert.Contains("'b'", message);
            Assert.Contains("'c'", message);
        }
    }

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
}
