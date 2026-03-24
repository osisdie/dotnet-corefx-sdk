using System;
using CoreFX.Abstractions.Bases;
using Xunit;

namespace CoreFX.Abstractions.Tests.Bases
{
    public class TestSingleton : LazySingleton<TestSingleton>
    {
        public bool WasInitialized { get; private set; }

        protected override void InitialInConstructor()
        {
            WasInitialized = true;
        }
    }

    public class LazySingletonTests
    {
        [Fact]
        public void Instance_ReturnsNonNull()
        {
            Assert.NotNull(TestSingleton.Instance);
        }

        [Fact]
        public void Instance_CalledTwice_ReturnsSameReference()
        {
            var first = TestSingleton.Instance;
            var second = TestSingleton.Instance;
            Assert.Same(first, second);
        }

        [Fact]
        public void Instance_HasUniqueGuid()
        {
            Assert.NotEqual(Guid.Empty, TestSingleton.Instance._id);
        }

        [Fact]
        public void Instance_WasInitialized()
        {
            Assert.True(TestSingleton.Instance.WasInitialized);
        }
    }
}
