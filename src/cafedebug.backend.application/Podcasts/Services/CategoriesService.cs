using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug.backend.application.Podcasts.Interfaces.Categories;
using cafedebug_backend.domain.Banners;
using cafedebug_backend.domain.Episodes.Repositories;
using cafedebug_backend.domain.Podcasts.Errors;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Services;

/// <summary>
/// Service responsible for managing categories, including creation, updating, deletion, and retrieval operations.
/// </summary>
public class CategoriesService(ICategoryRepository categoryRepository) : ICategoriesService
{
    public async Task<Result<CategoryResponse>> CreateAsync(CategoryRequest request)
    {
        var category = request.ToCategory();

        var exists = await categoryRepository.AnyAsync(e => e.Name == category.Name);

        if (exists)
            return Result.Failure<CategoryResponse>(CategoryError.AlreadyExists(request.Name));

        await categoryRepository.SaveAsync(category);

        var response = MappingConfig.ToCategory(category);
        return Result.Success(response);
    }
    public async Task<Result<CategoryResponse>> UpdateAsync(CategoryRequest request, int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category is null)
            return Result.Failure<CategoryResponse>(EpisodeError.NotFound(id));

        category.Update(request.Name);

        await categoryRepository.UpdateAsync(category);

        var response = MappingConfig.ToCategory(category);
        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category is null)
            return Result.Failure<Banner>(CategoryError.NotFound(id));

        await categoryRepository.DeleteAsync(category);
        return Result.Success();
    }

    public async Task<Result<PagedResult<CategoryResponse>>> GetAllAsync(PageRequest request)
    {
        var categories = await categoryRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);
        return categories.MapToPagedResult(category => category.ToCategory());
    }

    public async Task<Result<CategoryResponse>> GetByIdAsync(int id)
    {
        var category = await categoryRepository.GetByIdAsync(id);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryError.NotFound(id));

        var response = MappingConfig.ToCategory(category);
        return Result.Success(response);
    }

    public async Task<Result<CategoryResponse>> GetByNameAsync(string name)
    {
        var category = await categoryRepository.GetByNameAsync(name);

        if (category is null)
            return Result.Failure<CategoryResponse>(CategoryError.NotFound(name));

        var response = MappingConfig.ToCategory(category);
        return Result.Success(response);
    }
}