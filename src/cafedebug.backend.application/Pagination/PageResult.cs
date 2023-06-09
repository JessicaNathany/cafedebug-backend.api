namespace cafedebug.backend.application.Pagination
{
    public class PageResult<TResponse>
    {
        private PageResult(IEnumerable<TResponse> items, int page, int pageSize, int pageCount, int totalCount)
        {
            Data = items;
            Page = page;
            PageSize = pageSize;
            PageCount = pageCount;
            TotalCount = totalCount;
        }

        public IEnumerable<TResponse> Data { get; }
        public int Page { get; }
        public int PageSize { get; }
        public int PageCount { get; }
        public int TotalCount { get; }
        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < PageCount;

        public static PageResult<TResponse> Create(IEnumerable<TResponse> pagedResult, int page, int pageSize,
            int pageCount, int totalCount) => new(pagedResult, page, pageSize, pageCount, totalCount);
    }
}
