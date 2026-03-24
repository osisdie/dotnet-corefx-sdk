using System;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using Xunit;

namespace CoreFX.Abstractions.Tests.Contracts
{
    public class ResponseBaseDtoTests
    {
        [Fact]
        public void Constructor_SetsCodeToUnSet()
        {
            var dto = new ResponseBaseDto();
            Assert.Equal((int)SvcCodeEnum.UnSet, dto.Code);
        }

        [Fact]
        public void Constructor_SetsIsSuccessFalse()
        {
            var dto = new ResponseBaseDto();
            Assert.False(dto.IsSuccess);
        }

        [Fact]
        public void Constructor_GeneratesMsgId()
        {
            var dto = new ResponseBaseDto();
            Assert.True(Guid.TryParse(dto.MsgId, out _));
        }

        [Fact]
        public void Constructor_InitializesExtMap()
        {
            var dto = new ResponseBaseDto();
            Assert.NotNull(dto.ExtMap);
            Assert.True(dto.ExtMap.ContainsKey("ver"));
            Assert.True(dto.ExtMap.ContainsKey("env"));
        }

        [Fact]
        public void Constructor_SetsEmptyStrings()
        {
            var dto = new ResponseBaseDto();
            Assert.Equal(string.Empty, dto.Msg);
            Assert.Equal(string.Empty, dto.SubCode);
            Assert.Equal(string.Empty, dto.SubMsg);
        }

        [Fact]
        public void Initialize_CalledTwice_RegeneratesMsgId()
        {
            var dto = new ResponseBaseDto();
            var firstMsgId = dto.MsgId;
            dto.Initialize();
            Assert.NotEqual(firstMsgId, dto.MsgId);
        }
    }
}
