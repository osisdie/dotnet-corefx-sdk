using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CoreFX.Abstractions.Logging;
using CoreFX.Abstractions.Utils;
using CoreFX.Hosting.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Newtonsoft.Json;

namespace CoreFX.Hosting.Middlewares
{
    public class RequestResponseLogging_Middleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;

        public RequestResponseLogging_Middleware(RequestDelegate next, ILogger<RequestResponseLogging_Middleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            //await _next(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            var conversationSeqId = Guid.NewGuid().ToString();
            context.Items[SysLoggerKey.ConversationRootId] = conversationSeqId;
            context.Items[SysLoggerKey.ConversationSeqId] = conversationSeqId;
            context.Request.EnableBuffering();

            using (var requestStream = _recyclableMemoryStreamManager.GetStream())
            {
                await context.Request.Body.CopyToAsync(requestStream);

                var body = await ReadStreamInChunks(requestStream);
                var message = JsonConvert.SerializeObject(
                    context.Request.ToRequestDictInfo(reqBody: body, ext: new Dictionary<string, object>
                    {
                        { SysLoggerKey.ConversationRootId, conversationSeqId },
                        { SysLoggerKey.ConversationSeqId, conversationSeqId }
                    }), SerializerUtil.DefaultJsonSetting);

                _logger.LogInformation(message);
                context.Request.Body.Position = 0;
            }
        }

        private async Task LogResponse(HttpContext context)
        {
            var conversationRootId = context.Items[SysLoggerKey.ConversationRootId] ?? "";
            var conversationSeqId = Guid.NewGuid().ToString();
            var originalBodyStream = context.Response.Body;

            using (var responseBody = _recyclableMemoryStreamManager.GetStream())
            {
                context.Response.Body = responseBody;
                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var body = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                var message = JsonConvert.SerializeObject(
                    context.Response.ToResponseDictInfo(context.Request, respBody: body, ext: new Dictionary<string, object>
                    {
                        { SysLoggerKey.ConversationRootId, conversationRootId },
                        { SysLoggerKey.ConversationSeqId, conversationSeqId },
                    }), SerializerUtil.DefaultJsonSetting);

                _logger.LogInformation(message);
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;
            stream.Seek(0, SeekOrigin.Begin);

            using (var textWriter = new StringWriter())
            {
                using (var reader = new StreamReader(stream))
                {
                    var readChunk = new char[readChunkBufferLength];
                    int readChunkLength;
                    do
                    {
                        readChunkLength = await reader.ReadBlockAsync(readChunk, 0, readChunkBufferLength);
                        await textWriter.WriteLineAsync(readChunk, 0, readChunkLength);
                    } while (readChunkLength > 0);

                    return textWriter.ToString();
                }
            }
        }
    }
}
