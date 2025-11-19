using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Media.Errors;

public static class ImageError
{
    public static Error FailureToUploadImage => new(ErrorType.ErrorWhenExecuting, "Failure to upload image.");
    public static Error FailureToDeleteImage => new(ErrorType.ErrorWhenExecuting, "Failure to delete image.");
}