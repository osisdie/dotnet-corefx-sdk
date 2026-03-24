using System;
using System.Net;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Extensions;
using CoreFX.Hosting.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Xunit;

namespace CoreFX.Hosting.Tests.Extensions
{
    public class HttpResponseFormat_ExtensionTests
    {
        [Fact]
        public void ToErrorJson500_ContainsExceptionMessage()
        {
            var ex = new InvalidOperationException("something broke");
            var json = ex.ToErrorJson500();

            Assert.Contains("something broke", json);
        }

        [Fact]
        public void ToErrorJson500_ReturnsValidJson()
        {
            var ex = new Exception("test");
            var json = ex.ToErrorJson500();

            var parsed = JsonConvert.DeserializeObject<SvcResponseDto>(json);
            Assert.NotNull(parsed);
            Assert.Equal((int)SvcCodeEnum.Exception, parsed.Code);
            Assert.Equal((int)HttpStatusCode.InternalServerError, parsed.Errors.StatusCode);
        }

        [Fact]
        public void ToErrorJson400_InvalidModelState_ReturnsBadRequest()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("Field1", "Field1 is required");

            var result = modelState.ToErrorJson400();

            Assert.IsType<BadRequestObjectResult>(result);
            var value = result.Value as SvcResponseDto;
            Assert.NotNull(value);
            Assert.Equal((int)SvcCodeEnum.Error, value.Code);
            Assert.Equal((int)HttpStatusCode.BadRequest, value.Errors.StatusCode);
        }

        [Fact]
        public void ToErrorJson401_ReturnsUnauthorized()
        {
            var responseDto = new SvcResponseDto(false);
            responseDto.Msg = "not authorized";

            var result = responseDto.ToErrorJson401();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public void ToErrorMessage_EmptyModelState_ReturnsEmpty()
        {
            var modelState = new ModelStateDictionary();
            Assert.Equal(string.Empty, modelState.ToErrorMessage());
        }

        [Fact]
        public void ToErrorMessage_WithErrors_ReturnsJoinedMessages()
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("F1", "Error 1");
            modelState.AddModelError("F2", "Error 2");

            var message = modelState.ToErrorMessage();

            Assert.Contains("Error 1", message);
            Assert.Contains("Error 2", message);
        }
    }
}
