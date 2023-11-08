using cafedebug_backend.domain.Enums;
using Microsoft.Extensions.Localization;

namespace cafedebug_backend.domain.Errors
{
    public class UserError
    {
        public static Error NotFound(IStringLocalizer localizer) => new(ErrorType.ResourceNotFound, localizer["UserNotFoundError"]);
        public static Error Exists(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
        public static Error ErrorWhenRegister(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
    }
}
