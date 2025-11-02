namespace cafedebug_backend.domain.Shared;

public interface IPagedResult<out TEntity> : IReadOnlyList<TEntity>
{
    int Page { get; }
    int PageSize { get; }
    int PageCount { get; }
    int TotalCount { get; }
    string? SortBy { get; }
    bool Descending { get; }
}