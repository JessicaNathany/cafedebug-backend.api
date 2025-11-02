namespace cafedebug.backend.application.Common.Pagination;

public sealed record PagedResult<TResponse>
{
    private PagedResult(IReadOnlyList<TResponse> items, int page, int pageSize, int pageCount, int totalCount, string? sortBy = null, bool descending = false)
    {
        Items = items;
        Page = page;
        PageSize = pageSize;
        PageCount = pageCount;
        TotalCount = totalCount;
        SortBy = sortBy;
        Descending = descending;
    }

    public IReadOnlyList<TResponse> Items { get; }
    public int Page { get; }
    public int PageSize { get; }
    public int PageCount { get; }
    public int TotalCount { get; }
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < PageCount;
    public string? SortBy { get; }
    public bool Descending { get; }

    public static PagedResult<TResponse> Create(IReadOnlyList<TResponse> pagedResult, int page, int pageSize,
        int pageCount, int totalCount, string? sortBy = null, bool descending = false) => new(pagedResult, page, pageSize, pageCount, totalCount, sortBy, descending);
}