using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CoreFX.Hosting.Extensions
{
    public static class RequestResponseLogging_Middleware_Extension
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLogging_Middleware>();
        }
    }
}
