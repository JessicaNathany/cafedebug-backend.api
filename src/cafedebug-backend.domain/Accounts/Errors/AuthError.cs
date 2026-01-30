using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class AuthError
{
    public static Error InvalidCredentials()
    {
        return new Error(ErrorType.ResourceUnauthorized, $"Invalid email or password");
    }

    public static Error EmptyCredentials()
    {
        return new Error(ErrorType.ResourceUnauthorized, $"Email and password must not be empty");
    }
    public static Error TokenGenerationFailed(int userId)
    {
        return new Error(ErrorType.ErrorWhenExecuting, $"Error creating token for user with ID: {userId}");
    }

    public static Error RefreshTokenNull()
    {
        return new Error(ErrorType.BadRequest, "Refresh token cannot be null");
    }

    public static Error RefreshTokenInvalid()
    {
        return new Error(ErrorType.ResourceUnauthorized, "Invalid or expired refresh token");
    }

    public static Error RefreshTokenExpired()
    {
        return new Error(ErrorType.ResourceUnauthorized, "Refresh token has expired");
    }

    public static Error RefreshTokenGenerationFailed()
    {
        return new Error(ErrorType.ResourceUnauthorized, "Error creating new token");
    }
}