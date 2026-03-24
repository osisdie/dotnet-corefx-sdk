using System;
using CoreFX.Common.Extensions;
using Xunit;

namespace CoreFX.Common.Tests.Extensions
{
    public class String_ExtensionTests
    {
        #region ToInt

        [Fact]
        public void ToInt_ValidNumber_ReturnsParsed()
        {
            Assert.Equal(42, "42".ToInt());
        }

        [Fact]
        public void ToInt_NegativeNumber_ReturnsParsed()
        {
            Assert.Equal(-5, "-5".ToInt());
        }

        [Fact]
        public void ToInt_InvalidString_ReturnsDefault()
        {
            Assert.Equal(0, "abc".ToInt());
        }

        [Fact]
        public void ToInt_Null_ReturnsDefault()
        {
            Assert.Equal(0, ((string)null).ToInt());
        }

        [Fact]
        public void ToInt_CustomDefault_ReturnsCustom()
        {
            Assert.Equal(-1, "abc".ToInt(-1));
        }

        [Fact]
        public void ToInt_WithThousandsSeparator_ParsesCorrectly()
        {
            Assert.Equal(1000, "1,000".ToInt());
        }

        #endregion

        #region ToInt64

        [Fact]
        public void ToInt64_ValidNumber_ReturnsParsed()
        {
            Assert.Equal(9999999999L, "9999999999".ToInt64());
        }

        [Fact]
        public void ToInt64_InvalidString_ReturnsDefault()
        {
            Assert.Equal(0L, "abc".ToInt64());
        }

        [Fact]
        public void ToInt64_Null_ReturnsDefault()
        {
            Assert.Equal(0L, ((string)null).ToInt64());
        }

        #endregion

        #region ToBool

        [Theory]
        [InlineData("1")]
        [InlineData("t")]
        [InlineData("y")]
        [InlineData("yes")]
        [InlineData("true")]
        [InlineData("on")]
        public void ToBool_TrueStrings_ReturnsTrue(string input)
        {
            Assert.True(input.ToBool());
        }

        [Theory]
        [InlineData("0")]
        [InlineData("f")]
        [InlineData("n")]
        [InlineData("no")]
        [InlineData("false")]
        [InlineData("off")]
        public void ToBool_FalseStrings_ReturnsFalse(string input)
        {
            Assert.False(input.ToBool());
        }

        [Theory]
        [InlineData("TRUE")]
        [InlineData("Yes")]
        [InlineData("ON")]
        [InlineData("True")]
        public void ToBool_CaseInsensitive_ReturnsTrue(string input)
        {
            Assert.True(input.ToBool());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void ToBool_NullOrWhitespace_ReturnsDefault(string input)
        {
            Assert.False(input.ToBool());
        }

        [Fact]
        public void ToBool_UnrecognizedString_ReturnsDefault()
        {
            Assert.False("maybe".ToBool());
        }

        [Fact]
        public void ToBool_UnrecognizedString_ReturnsCustomDefault()
        {
            Assert.True("maybe".ToBool(true));
        }

        #endregion

        #region ToMD5

        [Fact]
        public void ToMD5_KnownInput_ReturnsExpected()
        {
            Assert.Equal("5D41402ABC4B2A76B9719D911017C592", "hello".ToMD5());
        }

        [Fact]
        public void ToMD5_EmptyString_ReturnsKnownHash()
        {
            Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", "".ToMD5());
        }

        [Fact]
        public void ToMD5_DeterministicOutput()
        {
            var hash1 = "test".ToMD5();
            var hash2 = "test".ToMD5();
            Assert.Equal(hash1, hash2);
        }

        #endregion

        #region MaskLeft / MaskRight

        [Fact]
        public void MaskLeft_Normal_MasksLeftHalf()
        {
            Assert.Equal("**34", "1234".MaskLeft());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void MaskLeft_NullOrEmpty_ReturnsEmpty(string input)
        {
            Assert.Equal(string.Empty, input.MaskLeft());
        }

        [Fact]
        public void MaskLeft_CustomChar_UsesCustomMask()
        {
            Assert.Equal("##34", "1234".MaskLeft('#'));
        }

        [Fact]
        public void MaskRight_Normal_MasksRightHalf()
        {
            Assert.Equal("12**", "1234".MaskRight());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void MaskRight_NullOrEmpty_ReturnsEmpty(string input)
        {
            Assert.Equal(string.Empty, input.MaskRight());
        }

        [Fact]
        public void MaskRight_CustomChar_UsesCustomMask()
        {
            Assert.Equal("12##", "1234".MaskRight('#'));
        }

        #endregion

        #region TrimStartSlash / TrimEndSlash

        [Fact]
        public void TrimStartSlash_RemovesLeadingSlash()
        {
            Assert.Equal("api/v1", "/api/v1".TrimStartSlash());
        }

        [Fact]
        public void TrimStartSlash_RemovesBackslash()
        {
            Assert.Equal("api", "\\api".TrimStartSlash());
        }

        [Fact]
        public void TrimStartSlash_NoSlash_ReturnsUnchanged()
        {
            Assert.Equal("api", "api".TrimStartSlash());
        }

        [Fact]
        public void TrimEndSlash_RemovesTrailingSlash()
        {
            Assert.Equal("api/v1", "api/v1/".TrimEndSlash());
        }

        [Fact]
        public void TrimEndSlash_NoSlash_ReturnsUnchanged()
        {
            Assert.Equal("api", "api".TrimEndSlash());
        }

        #endregion

        #region HasValue / IsNullOrEmpty

        [Fact]
        public void HasValue_NonEmpty_ReturnsTrue()
        {
            Assert.True("hello".HasValue());
        }

        [Fact]
        public void HasValue_WhitespaceOnly_ReturnsFalse()
        {
            Assert.False("   ".HasValue());
        }

        [Fact]
        public void HasValue_Null_ReturnsFalse()
        {
            Assert.False(((string)null).HasValue());
        }

        [Fact]
        public void IsNullOrEmpty_Null_ReturnsTrue()
        {
            Assert.True(((string)null).IsNullOrEmpty());
        }

        [Fact]
        public void IsNullOrEmpty_NonEmpty_ReturnsFalse()
        {
            Assert.False("hello".IsNullOrEmpty());
        }

        #endregion

        #region GrepFirst

        [Fact]
        public void GrepFirst_CommaSeparated_ReturnsFirstToken()
        {
            Assert.Equal("a", "a,b,c".GrepFirst());
        }

        [Fact]
        public void GrepFirst_SemicolonSeparated_ReturnsFirstToken()
        {
            Assert.Equal("x", "x;y".GrepFirst());
        }

        [Fact]
        public void GrepFirst_Null_ReturnsNull()
        {
            Assert.Null(((string)null).GrepFirst());
        }

        #endregion

        #region Contains

        [Fact]
        public void Contains_CaseInsensitive_ReturnsTrue()
        {
            Assert.True("Hello World".Contains("hello", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Contains_CaseSensitive_ReturnsFalse()
        {
            Assert.False("Hello World".Contains("hello", StringComparison.Ordinal));
        }

        #endregion

        #region ToDefaultIfEmpty / ToDefaultIfWhiteSpace

        [Fact]
        public void ToDefaultIfEmpty_Empty_ReturnsDefault()
        {
            Assert.Equal("fallback", "".ToDefaultIfEmpty("fallback"));
        }

        [Fact]
        public void ToDefaultIfEmpty_NonEmpty_ReturnsOriginal()
        {
            Assert.Equal("value", "value".ToDefaultIfEmpty("fallback"));
        }

        [Fact]
        public void ToDefaultIfWhiteSpace_Whitespace_ReturnsDefault()
        {
            Assert.Equal("fallback", "   ".ToDefaultIfWhiteSpace("fallback"));
        }

        [Fact]
        public void ToDefaultIfWhiteSpace_NonWhitespace_ReturnsOriginal()
        {
            Assert.Equal("value", "value".ToDefaultIfWhiteSpace("fallback"));
        }

        #endregion
    }
}
