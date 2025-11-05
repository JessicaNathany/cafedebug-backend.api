using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Podcasts.Errors;

public static class CategoryError
{
    public static Error NotFound(int categoryId)
    {
        return new Error(ErrorType.ResourceNotFound, $"Category with id {categoryId} not found");
    }
}