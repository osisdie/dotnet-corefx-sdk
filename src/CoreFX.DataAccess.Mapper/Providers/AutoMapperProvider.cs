using System.Collections.Generic;
using System.Linq;
using CoreFX.Abstractions.Mappers.Interfaces;

namespace CoreFX.DataAccess.Mapper.Providers
{
    public class AutoMapperProvider : IMapperMgr
    {
        public AutoMapperProvider(AutoMapper.IMapper mapper)
        {
            Mapper = mapper;
        }

        public TDestination ConvertTo<TDestination>(object source) =>
            Mapper.Map<TDestination>(source);

        public IEnumerable<TDestination> ConvertAll<TDestination>(IEnumerable<object> source) =>
            source
            ?.ToList()
            ?.ConvertAll(o => ConvertTo<TDestination>(o));

        public TDestination ConvertTo<TSource, TDestination>(TSource source) =>
            Mapper.Map<TSource, TDestination>(source);

        public IEnumerable<TDestination> ConvertAll<TSource, TDestination>(IEnumerable<TSource> source) =>
            source
            ?.ToList()
            ?.ConvertAll(o => ConvertTo<TSource, TDestination>(o));

        public void Dispose() { }

        /// <summary>
        /// The config for CreateMapper
        /// </summary>
        public AutoMapper.IMapper Mapper { get; private set; }
    }
}
