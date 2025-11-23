using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Common.Mappings
{
    public static class PagedResultMappingExtensions
    {
        public  static PagedResult<TDest> MapToPagedResult<TSource, TDest>(this IPagedResult<TSource> source, Func<TSource, TDest> map)
        {
            if(source is null)
                throw new ArgumentNullException(nameof(source));

            if(map is null)
                throw new ArgumentNullException(nameof(map));

            var items = source == null
                ? new List<TDest>()
                : source.Select(map).ToList();

            return PagedResult<TDest>.Create(
                items,
                source.Page,
                source.PageSize,
                source.PageCount,
                source.TotalCount,
                source.SortBy,
                source.Descending);
        }
    }
}
