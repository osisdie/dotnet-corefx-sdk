using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CoreFX.Hosting.Middlewares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace CoreFX.Hosting.Tests.Middlewares
{
    public class ExceptionHandler_MiddlewareTests
    {
        private readonly Mock<ILogger<ExceptionHandler_Middleware>> _logger;

        public ExceptionHandler_MiddlewareTests()
        {
            _logger = new Mock<ILogger<ExceptionHandler_Middleware>>();
        }

        [Fact]
        public async Task Invoke_NoException_CallsNext()
        {
            var nextCalled = false;
            RequestDelegate next = _ =>
            {
                nextCalled = true;
                return Task.CompletedTask;
            };

            var middleware = new ExceptionHandler_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.Invoke(context);

            Assert.True(nextCalled);
        }

        [Fact]
        public async Task Invoke_ExceptionThrown_Returns500()
        {
            RequestDelegate next = _ => throw new InvalidOperationException("test error");

            var middleware = new ExceptionHandler_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.Invoke(context);

            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
        }

        [Fact]
        public async Task Invoke_ExceptionThrown_SetsJsonContentType()
        {
            RequestDelegate next = _ => throw new Exception("fail");

            var middleware = new ExceptionHandler_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.Invoke(context);

            Assert.Equal("application/json", context.Response.ContentType);
        }

        [Fact]
        public async Task Invoke_ExceptionThrown_ResponseBodyContainsMessage()
        {
            RequestDelegate next = _ => throw new Exception("boom");

            var middleware = new ExceptionHandler_Middleware(next, _logger.Object);
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            await middleware.Invoke(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
            Assert.Contains("boom", body);
        }
    }
}
