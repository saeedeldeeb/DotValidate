using DotValidate.Attributes;
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
}
