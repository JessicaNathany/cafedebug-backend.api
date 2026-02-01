using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class TokenError
{
    public static Error PasswordInvalid()
    {
        return new Error(ErrorType.ResourceUnauthorized, $"User not found or invalid credentials.");
    }

    public static Error EmailOrPassordEmpty()
    {
        return new Error(ErrorType.ResourceUnauthorized, $"Email and password must not be empty.");
    }

    public static Error UserNotFound(string email)
    {
        return new Error(ErrorType.ResourceNotFound, $"Email and password must not be empty. {email}");
    }

    public static Error ErrorCreatingToken(string email)
    {
        return new Error(ErrorType.ResourceUnauthorized, $"Error creating token for user. {email}");
    }
}