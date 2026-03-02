using cafedebug.backend.application.Common.Pagination;
using cafedebug.backend.application.Podcasts.DTOs.Requests;
using cafedebug.backend.application.Podcasts.DTOs.Responses;
using cafedebug_backend.domain.Shared;

namespace cafedebug.backend.application.Podcasts.Interfaces.Categories;

public interface ICategoriesService
{
    Task<Result<CategoryResponse>> GetByIdAsync(int idn);
    Task<Result<PagedResult<CategoryResponse>>> GetAllAsync(PageRequest request);
    Task<Result<CategoryResponse>> CreateAsync(CategoryRequest category);
    Task<Result<CategoryResponse>> UpdateAsync(CategoryRequest category, int id);
    Task<Result> DeleteAsync(int id);
    Task<Result<CategoryResponse>> GetByNameAsync(string name);
}