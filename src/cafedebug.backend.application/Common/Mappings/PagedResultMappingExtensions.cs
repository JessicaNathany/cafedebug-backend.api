using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Common.Mappings;
public static class PagedResultMappingExtensions
{
    public  static PagedResult<TDest> MapToPagedResult<TSource, TDest>(this IPagedResult<TSource> source, Func<TSource, TDest> map)
    {
        ArgumentNullException.ThrowIfNull(source);

        ArgumentNullException.ThrowIfNull(map);

        var items = source.Select(map).ToList();

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
