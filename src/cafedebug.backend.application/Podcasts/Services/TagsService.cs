using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Tags;
using cafedebug_backend.domain.Podcasts.Errors;
using cafedebug_backend.domain.Podcasts.Repositories;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing tags, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class TagsService(ITagsRepository tagsRepository) : ITagsService
{
    public async Task<Result<TagsResponse>> CreateAsync(TagsRequest request)
    {
        var tags = request.ToTags();

        var exists = await tagsRepository.AnyAsync(e => e.Name == tags.Name);

        if (exists)
            return Result.Failure<TagsResponse>(TagsError.AlreadyExists(request.Name));

        await tagsRepository.SaveAsync(tags);

        var response = MappingConfig.ToTags(tags);
        return Result.Success(response);
    }

    public async Task<Result<TagsResponse>> UpdateAsync(int id, TagsRequest request)
    {
        var tags = await tagsRepository.GetByIdAsync(id);

        if (tags is null)
            return Result.Failure<TagsResponse>(TagsError.NotFound(id));

        tags.Update(request.Name);

        await tagsRepository.UpdateAsync(tags);

        var response = MappingConfig.ToTags(tags);
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var banner = await tagsRepository.GetByIdAsync(id);

        if (banner is null)
            return Result.Failure<TagsResponse>(TagsError.NotFound(id));

        await tagsRepository.DeleteAsync(banner);
        return Result.Success();
    }

    public async Task<Result<PagedResult<TagsResponse>>> GetAllAsync(PageRequest request)
    {
        var tags = await tagsRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);

        return tags.MapToPagedResult(tag => tag.ToTags());
    }

    public async Task<Result<TagsResponse>> GetByIdAsync(int id)
    {
        var tags = await tagsRepository.GetByIdAsync(id);

        if (tags is null)
            return Result.Failure<TagsResponse>(TagsError.NotFound(id));

        var response = MappingConfig.ToTags(tags);
        return Result.Success(response);
    }

    public async Task<Result<TagsResponse>> GetByNameAsync(string tagName)
    {
        var tags = await tagsRepository.GetByNameAsync(tagName);

        if (tags is null)
            return Result.Failure<TagsResponse>(TagsError.NotFound(tags.Id));

        var response = MappingConfig.ToTags(tags);
        return Result.Success(response);
    }
}

