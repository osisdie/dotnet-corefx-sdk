using System;
using CoreFX.Auth.Models;
using CoreFX.Auth.Tests.Helpers;
using CoreFX.Auth.Utils;
using Xunit;

namespace CoreFX.Auth.Tests.Utils
{
    public class JwtUtilTests
    {
        public JwtUtilTests()
        {
            JwtTestHelper.EnsureConfigured();
        }

        [Fact]
        public void GenTokenkey_ValidModel_ReturnsNonNull()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            var token = JwtUtil.GenTokenkey(model);
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public void GenTokenkey_NullModel_ReturnsNull()
        {
            var token = JwtUtil.GenTokenkey(null);
            Assert.Null(token);
        }

        [Fact]
        public void GenTokenkey_SetsExpirationOnModel()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            JwtUtil.GenTokenkey(model, 120);
            Assert.NotNull(model.Exp);
            Assert.True(model.Exp > DateTime.UtcNow);
        }

        [Fact]
        public void GenTokenkey_ExpireMinsBelowMinimum_DefaultsTo60()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            var before = DateTime.UtcNow;
            JwtUtil.GenTokenkey(model, 10);

            // expireMins <= 60 gets forced to 60, so Exp should be ~60 min from now
            var expectedMin = before.AddMinutes(59);
            var expectedMax = DateTime.UtcNow.AddMinutes(61);
            Assert.True(model.Exp >= expectedMin && model.Exp <= expectedMax,
                $"Exp {model.Exp} should be ~60 min from now");
        }

        [Fact]
        public void GenTokenkey_ModelWithoutId_GeneratesGuid()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            model._id = null;
            JwtUtil.GenTokenkey(model);
            Assert.NotNull(model._id);
            Assert.True(Guid.TryParse(model._id, out _));
        }

        [Fact]
        public void ExtracToken_ValidToken_ReturnsDto()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            var token = JwtUtil.GenTokenkey(model, 120);

            var extracted = JwtUtil.ExtracToken(token);

            Assert.NotNull(extracted);
            Assert.Equal(model.UserId, extracted.UserId);
            Assert.Equal(model.UserName, extracted.UserName);
            Assert.Equal(token, extracted.Token);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ExtracToken_NullOrEmpty_ReturnsNull(string token)
        {
            Assert.Null(JwtUtil.ExtracToken(token));
        }

        [Fact]
        public void ExtracToken_InvalidToken_ReturnsNull()
        {
            Assert.Null(JwtUtil.ExtracToken("invalid.token.value"));
        }

        [Fact]
        public void IsTokenValid_ValidToken_ReturnsTrue()
        {
            var model = JwtTestHelper.CreateTestTokenDto();
            var token = JwtUtil.GenTokenkey(model, 120);
            Assert.True(JwtUtil.IsTokenValid(token));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void IsTokenValid_NullOrEmpty_ReturnsFalse(string token)
        {
            Assert.False(JwtUtil.IsTokenValid(token));
        }

        [Fact]
        public void IsTokenValid_InvalidToken_ReturnsFalse()
        {
            Assert.False(JwtUtil.IsTokenValid("garbage.token.here"));
        }

        [Fact]
        public void AsDateTimeExac_UnixEpoch_ReturnsCorrectDate()
        {
            // 1609459200 = 2021-01-01T00:00:00Z
            var result = JwtUtil.AsDateTimeExac("1609459200");
            Assert.Equal(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc), result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void AsDateTimeExac_NullOrEmpty_ReturnsDefault(string input)
        {
            var defaultValue = new DateTime(2000, 1, 1);
            var result = JwtUtil.AsDateTimeExac(input, defaultValue);
            Assert.Equal(defaultValue, result);
        }

        [Fact]
        public void AsDateTimeExac_InvalidFormat_ReturnsDefault()
        {
            var defaultValue = new DateTime(2000, 1, 1);
            var result = JwtUtil.AsDateTimeExac("not-a-date", defaultValue);
            Assert.Equal(defaultValue, result);
        }
    }
}
