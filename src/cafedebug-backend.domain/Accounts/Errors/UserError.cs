using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class UserError
{
    public static Error NotFound(string email)
    {
        return new Error(ErrorType.ResourceNotFound, $"User not found. {email}");
    }

    public static Error UserAlreadyExists()
    {
        return new Error(ErrorType.ResourceNotFound, $"User already exists.");
    }

    public static Error InvalidPassword()
    {
        return new Error(ErrorType.ResourceNotFound, $"Invalid password");
    }
}