using cafedebug_backend.domain.Errors;

namespace cafedebug.backend.application.Common.Validations;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(ErrorType.ValidationError, "A validation problem occurred.");

    Error[] Errors { get; }

    IDictionary<string, string[]> ToDictionary();
}