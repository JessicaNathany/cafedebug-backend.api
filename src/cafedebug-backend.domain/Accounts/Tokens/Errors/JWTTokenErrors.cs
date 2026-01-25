using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Accounts.Tokens.Errors
{
    public class JWTTokenErrors
    {
        public static Error AlreadyExists(string message)
        {
            return new Error(ErrorType.BadRequest, $"Internal server error {message}");
        }

        public static Error NotFound(int userId)
        {
            return new Error(ErrorType.ResourceNotFound, $"User with id {userId} not found");
        }

        public static Error Unauthorized()
        {
            return new Error(ErrorType.ResourceUnauthorized, $"Invalid user or expired refresh token");
        }

        public static Error InvalidToken(string token)
        {
            return new Error(ErrorType.InvalidToken, $"The token {token} is invalid.");
        }
    }
}
