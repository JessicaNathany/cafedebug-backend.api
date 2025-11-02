using AutoMapper;
using cafedebug_backend.domain.Shared;
using cafedebug.backend.application.Common.Pagination;

namespace cafedebug.backend.application.Common.Mappings;

public static class MappingExtensions
{
    public static PagedResult<TResponse> MapToPagedResult<TResponse>(this IMapper mapper, IPagedResult<Entity> pagedResult)
    {
        var items = mapper.Map<IReadOnlyList<TResponse>>(pagedResult);
        
        return PagedResult<TResponse>.Create(items, pagedResult.Page, pagedResult.PageSize, pagedResult.PageCount, pagedResult.TotalCount);
    }
}