namespace cafedebug_backend.domain.Enums;

public enum ErrorType
{
    None,
    NullValue,
    RequiredValidation,
    ResourceNotFound,
    ValidationCustomMessage,
    ValidationError,
    ExistingRegister,
    ErrorWhenExecuting
}