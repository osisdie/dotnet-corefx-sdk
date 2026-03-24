using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoreFX.DataAccess.Mapper.Providers;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CoreFX.DataAccess.Mapper.Tests.Providers
{
    #region Test DTOs and Profile

    public class SourceDto
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    public class DestDto
    {
        public string? Name { get; set; }
        public int Value { get; set; }
    }

    public class TestMappingProfile : Profile
    {
        public TestMappingProfile()
        {
            CreateMap<SourceDto, DestDto>();
        }
    }

    #endregion

    public class AutoMapperProviderTests
    {
        private readonly AutoMapperProvider _provider;

        public AutoMapperProviderTests()
        {
            var cfg = new MapperConfigurationExpression();
            cfg.AddProfile<TestMappingProfile>();
            var config = new MapperConfiguration(cfg, NullLoggerFactory.Instance);
            var mapper = config.CreateMapper();
            _provider = new AutoMapperProvider(mapper);
        }

        [Fact]
        public void ConvertTo_ValidSource_MapsToDestination()
        {
            var source = new SourceDto { Name = "test", Value = 42 };
            var dest = _provider.ConvertTo<DestDto>(source);

            Assert.NotNull(dest);
            Assert.Equal("test", dest.Name);
            Assert.Equal(42, dest.Value);
        }

        [Fact]
        public void ConvertTo_TypedOverload_MapsCorrectly()
        {
            var source = new SourceDto { Name = "typed", Value = 99 };
            var dest = _provider.ConvertTo<SourceDto, DestDto>(source);

            Assert.NotNull(dest);
            Assert.Equal("typed", dest.Name);
            Assert.Equal(99, dest.Value);
        }

        [Fact]
        public void ConvertAll_MultipleItems_MapsAll()
        {
            var sources = new List<object>
            {
                new SourceDto { Name = "a", Value = 1 },
                new SourceDto { Name = "b", Value = 2 },
                new SourceDto { Name = "c", Value = 3 },
            };

            var results = _provider.ConvertAll<DestDto>(sources).ToList();

            Assert.Equal(3, results.Count);
            Assert.Equal("a", results[0].Name);
            Assert.Equal("b", results[1].Name);
            Assert.Equal("c", results[2].Name);
        }

        [Fact]
        public void ConvertAll_EmptyCollection_ReturnsEmpty()
        {
            var sources = new List<object>();
            var results = _provider.ConvertAll<DestDto>(sources);
            Assert.Empty(results);
        }

        [Fact]
        public void ConvertAll_NullCollection_ReturnsNull()
        {
            var results = _provider.ConvertAll<DestDto>(null);
            Assert.Null(results);
        }

        [Fact]
        public void ConvertAll_TypedOverload_MapsAllItems()
        {
            var sources = new List<SourceDto>
            {
                new SourceDto { Name = "x", Value = 10 },
                new SourceDto { Name = "y", Value = 20 },
            };

            var results = _provider.ConvertAll<SourceDto, DestDto>(sources).ToList();

            Assert.Equal(2, results.Count);
            Assert.Equal("x", results[0].Name);
            Assert.Equal("y", results[1].Name);
        }

        [Fact]
        public void Dispose_DoesNotThrow()
        {
            var exception = Record.Exception(() => _provider.Dispose());
            Assert.Null(exception);
        }
    }
}
