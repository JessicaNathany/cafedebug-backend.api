using cafedebug_backend.domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Pagination;

public class PagedList<T> : List<T>, IPagedResult<T>
{
    public PagedList(IQueryable<T> source, int page, int pageSize, string? sortBy, bool descending)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = source.Count();
        PageCount = GetPageCount(pageSize, TotalCount);
        SortBy = sortBy;
        Descending = descending;
        var skip = (Page - 1) * PageSize;

        AddRange(source.Skip(skip).Take(PageSize).ToList());
    }

    private PagedList(int totalCount, int page, int pageSize, string? sortBy, bool descending, IReadOnlyList<T> results)
    {
        Page = page;
        PageSize = pageSize;
        TotalCount = totalCount;
        PageCount = GetPageCount(pageSize, TotalCount);
        SortBy = sortBy;
        Descending = descending;
        AddRange(results);
    }

    public int Page { get; }
    public int PageSize { get; }
    public int PageCount { get; }
    public int TotalCount { get; }
    public string? SortBy { get; }
    public bool Descending { get; }

    private static int GetPageCount(int pageSize, int totalCount)
    {
        if (pageSize == 0) return 0;
        
        var remainder = totalCount % pageSize;
        return totalCount / pageSize + (remainder == 0 ? 0 : 1);
    }

    public static async Task<IPagedResult<T>> CreateAsync(IQueryable<T> source, int page, int pageSize, string? sortBy = null, bool descending = false, CancellationToken cancellationToken = default)
    {
        var count = await source.CountAsync(cancellationToken);
        var skip = (page - 1) * pageSize;

        var results = await source
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return new PagedList<T>(count, page, pageSize, sortBy, descending, results.AsReadOnly());
    }
}