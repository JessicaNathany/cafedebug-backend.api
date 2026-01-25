using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Errors
{
    public static class RefreshTokenError
    {
        public static Error RefreshTokenNullOrEmpty(string message)
        {
            return new Error(ErrorType.ResourceUnauthorized, $"Refresh token cannot be null. {message}");
        }

        public static Error InvalidOrExpiredRefreshToken(string message)
        {
            return new Error(ErrorType.ResourceUnauthorized, $"Refresh token is invalid or expired. {message}");
        }
    }
}
