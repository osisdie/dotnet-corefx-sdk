using CoreFX.Abstractions.Extensions;
using Xunit;

namespace CoreFX.Abstractions.Tests.Extensions
{
    public class Crypto_ExtensionTests
    {
        [Fact]
        public void ToMD5_KnownInput_ReturnsExpected()
        {
            Assert.Equal("5D41402ABC4B2A76B9719D911017C592", "hello".ToMD5());
        }

        [Fact]
        public void ToMD5_EmptyString_ReturnsKnownHash()
        {
            Assert.Equal("D41D8CD98F00B204E9800998ECF8427E", "".ToMD5());
        }

        [Fact]
        public void ToMD5_Deterministic()
        {
            var hash1 = "test-input".ToMD5();
            var hash2 = "test-input".ToMD5();
            Assert.Equal(hash1, hash2);
        }
    }
}
