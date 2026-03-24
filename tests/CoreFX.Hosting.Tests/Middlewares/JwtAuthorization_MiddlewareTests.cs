using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreFX.Abstractions.App_Start;
using CoreFX.Abstractions.Consts;
using CoreFX.Auth.Consts;
using CoreFX.Auth.Models;
using CoreFX.Auth.Utils;
using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoreFX.Hosting.Tests.Middlewares
{
    public class JwtAuthorization_MiddlewareTests
    {
        private readonly Mock<ILogger<JwtAuthorization_Middleware>> _logger;

        public JwtAuthorization_MiddlewareTests()
        {
            _logger = new Mock<ILogger<JwtAuthorization_Middleware>>();
            EnsureJwtConfig();
        }

        private static void EnsureJwtConfig()
        {
            if (SdkRuntime.Configuration == null)
            {
                SdkRuntime.Configuration = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        ["AuthConfig:JwtConfig:Secret"] = "ThisIsATestSecretKeyThatIsAtLeast32BytesLong!!",
                        ["AuthConfig:JwtConfig:Issuer"] = "test-issuer",
                        ["AuthConfig:JwtConfig:Audience"] = "test-audience",
                    })
                    .Build();
            }
        }

        [Fact]
        public async Task Invoke_NoAuthHeader_CallsNext()
        {
            var nextCalled = false;
            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new JwtAuthorization_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();

            await middleware.Invoke(context);

            Assert.True(nextCalled);
            Assert.False(context.Items.ContainsKey(JwtConst.JwtTokenItemName));
        }

        [Fact]
        public async Task Invoke_ValidBearerToken_SetsJwtTokenInItems()
        {
            var model = new JwtTokenDto { UserId = "user1", UserName = "Test" };
            var token = JwtUtil.GenTokenkey(model, 120);

            RequestDelegate next = _ => Task.CompletedTask;
            var middleware = new JwtAuthorization_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers[SvcConst.AuthHeaderName] = $"{JwtConst.JwtHeaderPrefix}{token}";

            await middleware.Invoke(context);

            Assert.True(context.Items.ContainsKey(JwtConst.JwtTokenItemName));
            Assert.Equal(token, context.Items[JwtConst.JwtTokenItemName]);
        }

        [Fact]
        public async Task Invoke_InvalidToken_DoesNotSetItems()
        {
            RequestDelegate next = _ => Task.CompletedTask;
            var middleware = new JwtAuthorization_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers[SvcConst.AuthHeaderName] = "Bearer invalid.token.here";

            await middleware.Invoke(context);

            Assert.False(context.Items.ContainsKey(JwtConst.JwtTokenItemName));
        }

        [Fact]
        public async Task Invoke_AlwaysCallsNext()
        {
            var nextCalled = false;
            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new JwtAuthorization_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Request.Headers[SvcConst.AuthHeaderName] = "Bearer garbage";

            await middleware.Invoke(context);

            Assert.True(nextCalled);
        }
    }
}
