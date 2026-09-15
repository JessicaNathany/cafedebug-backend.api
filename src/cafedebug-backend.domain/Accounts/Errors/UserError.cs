using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class UserError
{
    public static Error NotFound()
    {
        return new Error(ErrorType.ResourceNotFound, $"User not found.");
    }

    public static Error NotFound(string email)
    {
        return new Error(ErrorType.ResourceNotFound, $"User not found. {email}");
    }

    public static Error UserAlreadyExists(string email)
    {
        return new Error(ErrorType.ExistingRegister, $"User already exists. {email}");
    }

    public static Error InvalidPassword()
    {
        return new Error(ErrorType.ResourceNotFound, $"Invalid password");
    }

    public static Error EmailCannotBeNull()
    {
        return new Error(ErrorType.BadRequest, $"Email cannot be null");
    }

    public static Error NameCannotBeNull()
    {
        return new Error(ErrorType.BadRequest, "Name cannot be null");
    }

    public static Error PasswordCannotBeNull()
    {
        return new Error(ErrorType.BadRequest, "Password cannot be null");
    }

    public static Error InvalidEmailFormat()
    {
        return new Error(ErrorType.BadRequest, "Email is not in a correct format.");
    }
}