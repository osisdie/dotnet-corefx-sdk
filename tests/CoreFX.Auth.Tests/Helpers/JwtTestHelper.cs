using System.Collections.Generic;
using CoreFX.Abstractions.App_Start;
using CoreFX.Auth.Models;
using Microsoft.Extensions.Configuration;

namespace CoreFX.Auth.Tests.Helpers
{
    public static class JwtTestHelper
    {
        public const string TestSecret = "ThisIsATestSecretKeyThatIsAtLeast32BytesLong!!";
        public const string TestIssuer = "test-issuer";
        public const string TestAudience = "test-audience";

        public static IConfiguration BuildJwtConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["AuthConfig:JwtConfig:Secret"] = TestSecret,
                    ["AuthConfig:JwtConfig:Issuer"] = TestIssuer,
                    ["AuthConfig:JwtConfig:Audience"] = TestAudience,
                })
                .Build();
        }

        public static void EnsureConfigured()
        {
            if (SdkRuntime.Configuration == null)
            {
                SdkRuntime.Configuration = BuildJwtConfiguration();
            }
        }

        public static JwtTokenDto CreateTestTokenDto(string userId = "test-user", string userName = "TestUser")
        {
            return new JwtTokenDto
            {
                UserId = userId,
                UserName = userName,
            };
        }
    }
}
