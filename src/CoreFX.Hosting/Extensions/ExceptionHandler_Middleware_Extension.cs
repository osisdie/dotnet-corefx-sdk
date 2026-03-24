using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CoreFX.Hosting.Extensions
{
    public static class ExceptionHandler_Middleware_Extension
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler_Middleware>();
        }
    }
}
