using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Shared.Errors;

namespace cafedebug_backend.domain.Banners.Errors;

public static class BannerError
{
    public static Error AlreadyExists(string message)
    {
        return new Error(ErrorType.ExistingRegister, $"Already exists an banner with the title {message}");
    }

    public static Error NotFound(int bannerId)
    {
        return new Error(ErrorType.ResourceNotFound, $"Banner with id {bannerId} not found");
    }

    public static Error NotFound(string bannerName)
    {
        return new Error(ErrorType.ResourceNotFound, $"Banner with name {bannerName} not found");
    }
}