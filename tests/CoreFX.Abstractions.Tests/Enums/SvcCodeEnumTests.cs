using System;
using System.Linq;
using CoreFX.Abstractions.Enums;
using Xunit;

namespace CoreFX.Abstractions.Tests.Enums
{
    public class SvcCodeEnumTests
    {
        [Fact]
        public void UnSet_HasValueMinusOne()
        {
            Assert.Equal(-1, (int)SvcCodeEnum.UnSet);
        }

        [Fact]
        public void Error_HasValueZero()
        {
            Assert.Equal(0, (int)SvcCodeEnum.Error);
        }

        [Fact]
        public void Success_HasValueOne()
        {
            Assert.Equal(1, (int)SvcCodeEnum.Success);
        }

        [Fact]
        public void AllValues_AreDistinct()
        {
            var values = Enum.GetValues<SvcCodeEnum>().Cast<int>().ToList();
            Assert.Equal(values.Count, values.Distinct().Count());
        }
    }
}
