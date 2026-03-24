using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using CoreFX.Abstractions.App_Start;
using CoreFX.Abstractions.Bases.Interfaces;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Extensions;
using CoreFX.Abstractions.Logging;
using CoreFX.Abstractions.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace CoreFX.Hosting.Extensions
{
    public static class HttpResponseFormat_Extension
    {
        public static BadRequestObjectResult ToErrorJson400(this ModelStateDictionary src)
        {
            return new BadRequestObjectResult(new SvcResponseDto
            {
                Code = (int)SvcCodeEnum.Error,
                Errors = new ErrorDetailDto
                {
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Message = src.ToErrorMessage()
                }
            });
        }

        public static UnauthorizedObjectResult ToErrorJson401(this ISvcResponseBaseDto responseDto)
        {
            return new UnauthorizedObjectResult(new SvcResponseDto(responseDto)
            {
                Errors = new ErrorDetailDto
                {
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                }
            });
        }

        public static string ToErrorJson500(this Exception exception)
        {
            return JsonConvert.SerializeObject(new SvcResponseDto
            {
                Code = (int)SvcCodeEnum.Exception,
                Errors = new ErrorDetailDto
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = exception.Message,
                    Trace = SvcContext.IsDebug() ? exception.ToString() : null
                }
            }, SerializerUtil.DefaultJsonSetting);
        }

        public static string ToErrorMessage(this ModelStateDictionary src)
        {
            if (src?.Values.Count() > 0)
            {
                var errors = string.Join(Environment.NewLine, src.Values.Where(v => v.Errors.Count > 0)
                          .SelectMany(v => v.Errors)
                          .Select(v => v.ErrorMessage));

                return errors;
            }

            return string.Empty;
        }

        public static IDictionary<string, object> ToRequestDictInfo(this HttpRequest request,
            string @event = null,
            string reqBody = null,
            Dictionary<string, object> ext = null,
            [CallerMemberName] string caller = "")
        {
            var dicts = new Dictionary<string, object>
                {
                    { SysLoggerKey.EventType, @event ?? SysMetricKey.HTTPRequest },
                    { SysLoggerKey.RequestDto, new Dictionary<string, object>
                        {
                            { SysLoggerKey.Method, request?.Method },
                            { SysLoggerKey.HttpScheme, request?.Scheme },
                            { SysLoggerKey.HttpHost, request?.Host.ToString() },
                            { SysLoggerKey.HttpPath, request?.Path.ToString() },
                            { SysLoggerKey.HttpQueryString, request?.QueryString },
                            { SysLoggerKey.HttpBody, reqBody },
                            { SysLoggerKey.HttpHeaders, request?.Headers },
                        }
                    },
                    { SysLoggerKey.ProgramMethodName, caller },
                };

            if (ext?.Count > 0)
            {
                dicts = dicts.Union(ext).GroupBy(d => d.Key)
                    .ToDictionary(d => d.Key, d => d.First().Value);
            }

            return dicts.AddDebugData();
        }

        public static IDictionary<string, object> ToResponseDictInfo(this HttpResponse response,
            HttpRequest request = null,
            string @event = null,
            string respBody = null,
            Dictionary<string, object> ext = null,
           [CallerMemberName] string caller = "")
        {
            var dicts = new Dictionary<string, object>
                {
                    { SysLoggerKey.EventType, @event ?? SysMetricKey.HTTPResponse },
                    { SysLoggerKey.ResponseDto, new Dictionary<string, object>
                        {
                            { SysLoggerKey.HttpBody, respBody },
                            { SysLoggerKey.HttpHeaders, response?.Headers },
                        }
                    },
                    { SysLoggerKey.RequestDto, new Dictionary<string, object>
                        {
                            { SysLoggerKey.Method, request?.Method },
                            { SysLoggerKey.HttpScheme, request?.Scheme },
                            { SysLoggerKey.HttpHost, request?.Host.ToString() },
                            { SysLoggerKey.HttpPath, request?.Path.ToString() },
                            { SysLoggerKey.HttpQueryString, request?.QueryString },
                            { SysLoggerKey.HttpHeaders, request?.Headers },
                            //{ SysLoggerKey.HttpBody, request?.Body.ReadToEndAsync() },
                        }
                    },
                    { SysLoggerKey.ProgramMethodName, caller },
                };

            if (ext?.Count > 0)
            {
                dicts = dicts.Union(ext).GroupBy(d => d.Key)
                    .ToDictionary(d => d.Key, d => d.First().Value);
            }

            return dicts.AddDebugData();
        }
    }
}
