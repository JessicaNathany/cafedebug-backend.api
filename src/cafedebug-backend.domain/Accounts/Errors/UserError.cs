using cafedebug_backend.domain.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class UserError
{
    public static Error NotFound => new(ErrorType.ResourceNotFound, "User not found");
    public static Error AlreadyExists => new(ErrorType.ExistingRegister, "User already exists");
    public static Error InvalidPassword => new(ErrorType.ValidationError, "Invalid password");
}