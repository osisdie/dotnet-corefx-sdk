using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CoreFX.Logging.Log4net.Tests
{
    public class Log4netAdapterTests
    {
        private readonly Log4netAdapter _adapter;

        public Log4netAdapterTests()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            _adapter = new Log4netAdapter("TestAdapter", new FileInfo(configPath));
        }

        [Theory]
        [InlineData(LogLevel.Debug)]
        [InlineData(LogLevel.Trace)]
        [InlineData(LogLevel.Information)]
        [InlineData(LogLevel.Warning)]
        [InlineData(LogLevel.Error)]
        [InlineData(LogLevel.Critical)]
        public void IsEnabled_KnownLevels_ReturnsExpected(LogLevel level)
        {
            // With DEBUG root level, all should be enabled
            var result = _adapter.IsEnabled(level);
            Assert.True(result);
        }

        [Fact]
        public void IsEnabled_NoneLevel_ReturnsFalse()
        {
            Assert.False(_adapter.IsEnabled(LogLevel.None));
        }

        [Fact]
        public void Log_NullFormatter_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _adapter.Log(LogLevel.Information, new EventId(1), "state", null,
                    (Func<string, Exception, string>)null!));
        }

        [Fact]
        public void Log_ValidMessage_DoesNotThrow()
        {
            var exception = Record.Exception(() =>
                _adapter.Log(LogLevel.Information, new EventId(1), "test message", null,
                    (state, ex) => state));
            Assert.Null(exception);
        }

        [Fact]
        public void BeginScope_ReturnsNull()
        {
            var scope = _adapter.BeginScope("test scope");
            Assert.Null(scope);
        }

        [Fact]
        public void Logger_Property_IsNotNull()
        {
            Assert.NotNull(_adapter.Logger);
        }
    }
}
