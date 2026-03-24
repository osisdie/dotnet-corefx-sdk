using System.Threading.Tasks;
using CoreFX.Abstractions.Extensions;
using CoreFX.Auth.Services;
using CoreFX.Auth.Tests.Helpers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoreFX.Auth.Tests.Services
{
    public class SessionAdminTests
    {
        private readonly SessionAdmin _sessionAdmin;

        public SessionAdminTests()
        {
            JwtTestHelper.EnsureConfigured();
            var logger = new Mock<ILogger<SessionAdmin>>();
            _sessionAdmin = new SessionAdmin(logger.Object);
        }

        [Fact]
        public async Task Authentication_ValidCredentials_ReturnsSuccess()
        {
            var result = await _sessionAdmin.Authentication("admin", "password");
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Authentication_PopulatesTokenFields()
        {
            var result = await _sessionAdmin.Authentication("admin", "password");
            Assert.NotNull(result.Data);
            Assert.NotNull(result.Data.Token);
            Assert.NotNull(result.Data.RefreshToken);
            Assert.NotNull(result.Data.UserId);
            Assert.NotNull(result.Data.Exp);
        }

        [Fact]
        public async Task Authentication_UserIdIsMD5OfUsername()
        {
            var username = "testuser";
            var result = await _sessionAdmin.Authentication(username, "password");

            var expectedMd5 = Crypto_Extension.ToMD5(username);
            Assert.Equal(expectedMd5, result.Data.UserId);
        }

        [Fact]
        public async Task Authentication_TokenAndRefreshTokenAreDifferent()
        {
            var result = await _sessionAdmin.Authentication("admin", "password");
            Assert.NotEqual(result.Data.Token, result.Data.RefreshToken);
        }

        [Fact]
        public void RefeshToken_Null_ReturnsFailure()
        {
            var result = _sessionAdmin.RefeshToken(null);
            Assert.False(result.IsSuccess);
            Assert.Equal("token does not exist.", result.Msg);
        }

        [Fact]
        public void RefeshToken_EmptyString_ReturnsFailure()
        {
            var result = _sessionAdmin.RefeshToken("");
            Assert.False(result.IsSuccess);
            Assert.Equal("token does not exist.", result.Msg);
        }

        [Fact]
        public void RefeshToken_InvalidToken_ReturnsFailure()
        {
            var result = _sessionAdmin.RefeshToken("invalid.token");
            Assert.False(result.IsSuccess);
            Assert.Equal("token invalid.", result.Msg);
        }

        [Fact]
        public async Task RefeshToken_ValidToken_ReturnsNewTokens()
        {
            // First authenticate to get a valid refresh token
            var authResult = await _sessionAdmin.Authentication("admin", "password");
            var refreshToken = authResult.Data.RefreshToken;

            var refreshResult = _sessionAdmin.RefeshToken(refreshToken);

            Assert.True(refreshResult.IsSuccess);
            Assert.NotNull(refreshResult.Data.Token);
            Assert.NotNull(refreshResult.Data.RefreshToken);
        }
    }
}
