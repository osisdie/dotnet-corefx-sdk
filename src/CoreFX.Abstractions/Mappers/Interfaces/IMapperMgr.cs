using System.Collections.Generic;

namespace CoreFX.Abstractions.Mappers.Interfaces
{
    public interface IMapperMgr
    {
        TDestination ConvertTo<TDestination>(object source);
        IEnumerable<TDestination> ConvertAll<TDestination>(IEnumerable<object> source);
        TDestination ConvertTo<TSource, TDestination>(TSource source);
        IEnumerable<TDestination> ConvertAll<TSource, TDestination>(IEnumerable<TSource> source);
    }
}
