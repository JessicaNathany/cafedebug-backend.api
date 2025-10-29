using cafedebug_backend.domain.Errors;

namespace cafedebug_backend.domain.Accounts.Errors;

public static class TokenError
{
    public static Error NotFound => new Error(ErrorType.ResourceNotFound, "Token not found");
}