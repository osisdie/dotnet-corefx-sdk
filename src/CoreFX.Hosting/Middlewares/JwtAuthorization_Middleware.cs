using System;
using System.Linq;
using System.Threading.Tasks;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Utils;
using CoreFX.Auth.Consts;
using CoreFX.Auth.Utils;
using CoreFX.Hosting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CoreFX.Hosting.Middlewares
{
    public class JwtAuthorization_Middleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public JwtAuthorization_Middleware(RequestDelegate next, ILogger<JwtAuthorization_Middleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers[SvcConst.AuthHeaderName].FirstOrDefault();
            if (token?.StartsWith(JwtConst.JwtHeaderPrefix, StringComparison.OrdinalIgnoreCase) == true)
            {
                token = token.Substring(JwtConst.JwtHeaderPrefix.Length).Trim();
            }

            if (!string.IsNullOrEmpty(token))
            {
                // Extract value from token and validate 
                var extractTokenObjet = JwtUtil.ExtracToken(token);
                if (extractTokenObjet != null && extractTokenObjet.Exp > DateTime.UtcNow)
                {
                    context.Items[JwtConst.JwtTokenItemName] = token;
                }
                else if (extractTokenObjet != null)
                {
                    var message = JsonConvert.SerializeObject(
                        context.Request.ToRequestDictInfo(@event: JwtConst.JwtExpiredMsg),
                        SerializerUtil.DefaultJsonSetting);

                    _logger.LogWarning(message);
                }
                else
                {
                    var message = JsonConvert.SerializeObject(
                        context.Request.ToRequestDictInfo(@event: JwtConst.JwtInvalidMsg),
                        SerializerUtil.DefaultJsonSetting);

                    _logger.LogWarning(message);
                }
            }

            return _next(context);
        }
    }
}
