namespace cafedebug_backend.domain.Errors;

public enum ErrorType
{
    None,
    NullValue,
    RequiredValidation,
    ResourceNotFound,
    ValidationCustomMessage,
    ValidationError,
    ExistingRegister,
    ErrorWhenExecuting,
    Unauthorized,
    Forbidden
}