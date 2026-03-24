using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using Xunit;

namespace CoreFX.Abstractions.Tests.Contracts
{
    public class SvcResponseDtoTests
    {
        [Fact]
        public void Constructor_WithTrue_SetsSuccess()
        {
            var dto = new SvcResponseDto(true);
            Assert.True(dto.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.Success, dto.Code);
        }

        [Fact]
        public void Constructor_WithFalse_SetsError()
        {
            var dto = new SvcResponseDto(false);
            Assert.False(dto.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.Error, dto.Code);
        }

        [Fact]
        public void CopyConstructor_CopiesFields()
        {
            var source = new SvcResponseDto(true);
            source.Msg = "hello";
            source.SubCode = "42";
            source.SubMsg = "sub";

            var copy = new SvcResponseDto(source);

            Assert.Equal(source.Code, copy.Code);
            Assert.Equal(source.Msg, copy.Msg);
            Assert.Equal(source.SubCode, copy.SubCode);
            Assert.Equal(source.SubMsg, copy.SubMsg);
            Assert.Equal(source.IsSuccess, copy.IsSuccess);
        }

        [Fact]
        public void CopyConstructor_Null_KeepsDefaults()
        {
            var dto = new SvcResponseDto((SvcResponseDto)null);
            Assert.Equal((int)SvcCodeEnum.UnSet, dto.Code);
            Assert.False(dto.IsSuccess);
        }

        [Fact]
        public void GenericConstructor_WithData_SetsData()
        {
            var dto = new SvcResponseDto<string>(true, "payload");
            Assert.True(dto.IsSuccess);
            Assert.Equal("payload", dto.Data);
        }

        [Fact]
        public void GenericConstructor_Default_HasNullData()
        {
            var dto = new SvcResponseDto<string>();
            Assert.Null(dto.Data);
        }
    }
}
