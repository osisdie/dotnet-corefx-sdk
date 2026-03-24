using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CoreFX.Hosting.Extensions
{
    public static class JwtAuthorization_Middleware_Extensions
    {
        public static IApplicationBuilder UseJwtAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthorization_Middleware>();
        }
    }
}
