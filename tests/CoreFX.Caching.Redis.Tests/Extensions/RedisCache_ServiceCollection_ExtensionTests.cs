using System;
using CoreFX.Caching.Redis.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CoreFX.Caching.Redis.Tests.Extensions
{
    public class RedisCache_ServiceCollection_ExtensionTests
    {
        [Fact]
        public void AddRedisCache_EmptyConnectionString_DoesNotThrow()
        {
            var services = new ServiceCollection();
            var exception = Record.Exception(() => services.AddRedisCache(""));
            Assert.Null(exception);
        }

        [Fact]
        public void AddRedisCache_NullConnectionString_DoesNotThrow()
        {
            var services = new ServiceCollection();
            var exception = Record.Exception(() => services.AddRedisCache(null));
            Assert.Null(exception);
        }

        [Fact]
        public void AddRedisCache_ReturnsServiceCollection()
        {
            var services = new ServiceCollection();
            var result = services.AddRedisCache("");
            Assert.Same(services, result);
        }

        [Fact]
        public void AddRedisCache_ValidConnectionString_RegistersDistributedCache()
        {
            var services = new ServiceCollection();
            // Use a valid connection string format (won't actually connect during registration)
            services.AddRedisCache("localhost:6379");

            var descriptor = Assert.Single(services, s => s.ServiceType == typeof(IDistributedCache));
            Assert.NotNull(descriptor);
        }
    }
}
