using System.Linq;
using CoreFX.Abstractions.Consts;
using CoreFX.Auth.Models;
using CoreFX.Auth.Utils;
using Microsoft.AspNetCore.Http;

namespace CoreFX.Hosting.Extensions
{
    public static class JwtToken_Extension
    {
        public static string GetTokenValue(this HttpContext src) =>
            src.Request.Headers[SvcConst.AuthHeaderName].FirstOrDefault()?.Split(" ").Last();

        public static JwtTokenDto GetTokenDto(this HttpContext src)
        {
            var token = src.GetTokenValue();
            if (token != null)
            {
                return JwtUtil.ExtracToken(token);
            }

            return null;
        }
    }
}
