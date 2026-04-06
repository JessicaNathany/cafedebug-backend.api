using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Interfaces.Tags
{
    public interface ITagsService
    {
        Task<Result<TagsResponse>> CreateAsync(TagsRequest request);
        Task<Result<TagsResponse>> UpdateAsync(int id, TagsRequest request);
        Task<Result> DeleteAsync(int id);
        Task<Result<TagsResponse>> GetByIdAsync(int id);
        Task<Result<TagsResponse>> GetByNameAsync(string tagName);
        Task<Result<PagedResult<TagsResponse>>> GetAllAsync(PageRequest request);
    }
}
