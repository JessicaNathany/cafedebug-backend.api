namespace cafedebug.backend.application.Common.Pagination;

public sealed record PageRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; } = null;
    
    public bool Descending { get; set; } = false;
}