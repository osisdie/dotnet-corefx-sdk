using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Extensions;
using Xunit;

namespace CoreFX.Abstractions.Tests.Extensions
{
    public class SvcResponse_ExtensionTests
    {
        [Fact]
        public void Success_SetsCodeOneAndIsSuccessTrue()
        {
            var res = new SvcResponseDto();
            res.Success();
            Assert.True(res.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.Success, res.Code);
        }

        [Fact]
        public void Error_SetsCodeZeroAndIsSuccessFalse()
        {
            var res = new SvcResponseDto();
            res.Error();
            Assert.False(res.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.Error, res.Code);
        }

        [Fact]
        public void Error_WithEnumAndMessage_SetsCodeAndMsg()
        {
            var res = new SvcResponseDto();
            res.Error(SvcCodeEnum.Exception, "fail");
            Assert.Equal((int)SvcCodeEnum.Exception, res.Code);
            Assert.Equal("fail", res.Msg);
            Assert.False(res.IsSuccess);
        }

        [Fact]
        public void Error_WithEnumAndException_UsesExceptionMessage()
        {
            var res = new SvcResponseDto();
            res.Error(SvcCodeEnum.Exception, new System.Exception("boom"));
            Assert.Equal("boom", res.Msg);
        }

        [Fact]
        public void SetData_SetsDataProperty()
        {
            var res = new SvcResponseDto<string>();
            res.SetData("val");
            Assert.Equal("val", res.Data);
        }

        [Fact]
        public void SetMsg_SetsMessageProperty()
        {
            var res = new SvcResponseDto<string>();
            res.SetMsg("hello");
            Assert.Equal("hello", res.Msg);
        }

        [Fact]
        public void SetStatusCode_True_CallsSuccess()
        {
            var res = new SvcResponseDto<string>();
            res.SetStatusCode(true);
            Assert.True(res.IsSuccess);
        }

        [Fact]
        public void SetStatusCode_False_CallsError()
        {
            var res = new SvcResponseDto<string>();
            res.SetStatusCode(false);
            Assert.False(res.IsSuccess);
        }

        [Fact]
        public void Any_SuccessWithData_ReturnsTrue()
        {
            var res = new SvcResponseDto<string>(true, "data");
            Assert.True(res.Any());
        }

        [Fact]
        public void Any_SuccessWithNullData_ReturnsFalse()
        {
            var res = new SvcResponseDto<string>(true);
            Assert.False(res.Any());
        }

        [Fact]
        public void Any_FailureWithData_ReturnsFalse()
        {
            var res = new SvcResponseDto<string>(false);
            res.Data = "data";
            Assert.False(res.Any());
        }

        [Fact]
        public void FailOrEmpty_FailureWithData_ReturnsTrue()
        {
            var res = new SvcResponseDto<string>(false);
            res.Data = "data";
            Assert.True(res.FailOrEmpty());
        }

        [Fact]
        public void FailOrEmpty_SuccessWithData_ReturnsFalse()
        {
            var res = new SvcResponseDto<string>(true, "data");
            Assert.False(res.FailOrEmpty());
        }

        [Fact]
        public void FailOrEmpty_SuccessWithNullData_ReturnsTrue()
        {
            var res = new SvcResponseDto<string>(true);
            Assert.True(res.FailOrEmpty());
        }

        [Fact]
        public void FluentChaining_AllApplied()
        {
            var res = new SvcResponseDto<string>();
            res.Success().SetData("x").SetMsg("done");
            Assert.True(res.IsSuccess);
            Assert.Equal("x", res.Data);
            Assert.Equal("done", res.Msg);
        }

        [Fact]
        public void SetSubCode_WithEnum_SetsSubCodeAndSubMsg()
        {
            var res = new SvcResponseDto<string>();
            res.SetSubCode(SvcCodeEnum.TokenExpired);
            Assert.Equal(((int)SvcCodeEnum.TokenExpired).ToString(), res.SubCode);
            Assert.Equal(SvcCodeEnum.TokenExpired.ToString(), res.SubMsg);
        }
    }
}
