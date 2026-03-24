using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using CoreFX.Abstractions.App_Start;
using CoreFX.Abstractions.Extensions;
using CoreFX.Auth.Consts;
using CoreFX.Auth.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CoreFX.Auth.Utils
{
    public static class JwtUtil
    {
        private static IEnumerable<Claim> GetAdminClaims(JwtTokenDto model)
        {
            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(JwtClaimType.UserId, model.UserId),
                new Claim(ClaimTypes.Name, model.UserName),
                //new Claim(JwtClaimType.DisplayName, model.FullName),
                //new Claim(JwtClaimType.EmailAddress, model.Email),
                new Claim(JwtClaimType.GuidId, model._id),
                new Claim(JwtClaimType.ExpiredTime, model.Exp?.ToString("s"))
            };

            return claims;
        }

        public static string GenTokenkey(JwtTokenDto model, int expireMins = 60)
        {
            if (model == null)
                return null;

            string secret = SdkRuntime.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Secret")
                ?? throw new ArgumentNullException("secret");
            string issuer = SdkRuntime.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Issuer");
            string audience = SdkRuntime.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Audience");

            try
            {
                // secret key
                var key = Encoding.ASCII.GetBytes(secret);

                expireMins = expireMins <= 60 ? 60 : expireMins;
                var expireTime = DateTime.UtcNow.AddMinutes(expireMins);
                model.Exp = expireTime;

                // Generate new guid string help determine user login
                if (string.IsNullOrEmpty(model._id))
                {
                    model._id = Guid.NewGuid().ToString();
                }

                //Generate Token for user 
                var JWToken = new JwtSecurityToken(
                    issuer: issuer,
                    audience: audience,
                    claims: GetAdminClaims(model),
                    notBefore: DateTime.UtcNow,
                    expires: expireTime,
                    //Using HS256 Algorithm to encrypt Token  
                    signingCredentials: new SigningCredentials
                    (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );
                var token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                return token;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static JwtTokenDto ExtracToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            string secret = SvcContext.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Secret")
                ?? throw new ArgumentNullException("secret");
            string issuer = SvcContext.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Issuer");
            string audience = SvcContext.Configuration?.GetValue<string>("AuthConfig:JwtConfig:Audience");

            try
            {

                var key = Encoding.ASCII.GetBytes(secret);
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    //LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
                    //                 TokenValidationParameters validationParameters) =>
                    //{
                    //    return notBefore <= DateTime.UtcNow && expires >= DateTime.UtcNow;
                    //}
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var adminId = jwtToken.Claims.FirstOrDefault(x => x.Type.Contains("sub"))?.Value;
                var userName = jwtToken.Claims.FirstOrDefault(x => x.Type.Contains("name"))?.Value;
                //string emailaddress = jwtToken.Claims.FirstOrDefault(x => x.Type.Contains("email"))?.Value;
                var guidId = jwtToken.Claims.FirstOrDefault(x => x.Type.Contains("jti"))?.Value;
                var expiredTime = jwtToken.Claims.FirstOrDefault(x => x.Type.Contains("exp")).Value.AsDateTimeExac(DateTime.UtcNow);

                return new JwtTokenDto()
                {
                    UserId = adminId,
                    UserName = userName,
                    //FullName = displayName,
                    //Email = emailaddress,
                    Exp = expiredTime,
                    Token = token,
                    _id = guidId,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static readonly string[] formats = { 
            // Basic formats
            "yyyyMMddTHHmmsszzz",
            "yyyyMMddTHHmmsszz",
            "yyyyMMddTHHmmssZ",
            // Extended formats
            "yyyy-MM-ddTHH:mm:sszzz",
            "yyyy-MM-ddTHH:mm:sszz",
            "yyyy-MM-ddTHH:mm:ssZ",
            // All of the above with reduced accuracy
            "yyyyMMddTHHmmzzz",
            "yyyyMMddTHHmmzz",
            "yyyyMMddTHHmmZ",
            "yyyy-MM-ddTHH:mmzzz",
            "yyyy-MM-ddTHH:mmzz",
            "yyyy-MM-ddTHH:mmZ",
            // Accuracy reduced to hours
            "yyyyMMddTHHzzz",
            "yyyyMMddTHHzz",
            "yyyyMMddTHHZ",
            "yyyy-MM-ddTHHzzz",
            "yyyy-MM-ddTHHzz",
            "yyyy-MM-ddTHHZ"
        };

        public static DateTime AsDateTimeExac(this object obj, DateTime defaultValue = default)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
            {
                return defaultValue;
            }

            DateTime result;
            if (long.TryParse(obj.ToString(), out var epoch))
            {
                return epoch.FromUnixTime();
            }

            if (!DateTime.TryParseExact(obj.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
            {
                return defaultValue;
            }

            return result;
        }

        public static bool IsTokenValid(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var payload = ExtracToken(token);
            if (payload == null)
            {
                return false;
            }

            return true;
        }
    }
}
