using cafedebug_backend.domain.Errors;

namespace cafedebug_backend.domain.Episodes.Errors;

public static class CategoryError
{
    public static Error NotFound(Guid categoryId)
    {
        return new Error(ErrorType.ResourceNotFound, $"Category with id {categoryId} not found");
    }
}