using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Podcasts.Errors;

public static class CategoryError
{
    public static Error AlreadyExists(string message)
    {
        return new Error(ErrorType.ExistingRegister, $"Already exists an category with the title {message}");
    }

    public static Error NotFound(int categoryId)
    {
        return new Error(ErrorType.ResourceNotFound, $"Category with id {categoryId} not found");
    }

    public static Error NotFound(string categoryName)
    {
        return new Error(ErrorType.ResourceNotFound, $"Category with name {categoryName} not found");
    }
}
