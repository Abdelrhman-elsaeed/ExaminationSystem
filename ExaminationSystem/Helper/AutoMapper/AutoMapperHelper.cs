using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ExaminationSystem.Helper.AutoMapper
{
    public static class AutoMapperHelper
    {
        public static IMapper Mapper { get; set; }

        // for one object
        public static T Map<T>(this object source)
        {
            return Mapper.Map<T>(source);
        }

        //multiple objects
        public static IQueryable<T> Project<T>(this IQueryable<object> source)
        {
            return source.ProjectTo<T>(Mapper.ConfigurationProvider);
        }
    }

}
