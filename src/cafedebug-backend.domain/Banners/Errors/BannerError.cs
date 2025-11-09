using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Banners.Errors;

public static class BannerError
{
    public static Error NotFound(int id) => new(ErrorType.ResourceNotFound, $"Banner not found with id: {id}");
    public static Error AlreadyExists => new(ErrorType.ExistingRegister, "Banner already exists");
}