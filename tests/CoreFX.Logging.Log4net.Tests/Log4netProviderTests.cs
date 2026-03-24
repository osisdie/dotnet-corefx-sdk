using System.IO;
using Microsoft.Extensions.Logging;
using Xunit;

namespace CoreFX.Logging.Log4net.Tests
{
    public class Log4netProviderTests
    {
        private readonly string _configPath;

        public Log4netProviderTests()
        {
            _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
        }

        [Fact]
        public void Constructor_SetsProviderFile()
        {
            using var provider = new Log4netProvider(_configPath);
            Assert.NotNull(provider.ProviderFile);
            Assert.Equal("log4net.config", provider.ProviderFile.Name);
        }

        [Fact]
        public void CreateLogger_ReturnsLog4netAdapter()
        {
            using var provider = new Log4netProvider(_configPath);
            var logger = provider.CreateLogger("TestLogger");

            Assert.NotNull(logger);
            Assert.IsType<Log4netAdapter>(logger);
        }

        [Fact]
        public void Dispose_DoesNotThrow()
        {
            var provider = new Log4netProvider(_configPath);
            var exception = Record.Exception(() => provider.Dispose());
            Assert.Null(exception);
        }
    }
}
