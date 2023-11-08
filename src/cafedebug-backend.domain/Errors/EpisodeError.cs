using cafedebug_backend.domain.Enums;
using cafedebug_backend.domain.Errors;
using cafedebug_backend.domain.Resources;
using Microsoft.Extensions.Localization;
using System.Resources;

namespace cafedebug_backend.domain.Erros
{
    public class EpisodeError
    {
        public static Error NotFound(IStringLocalizer localizer) => new(ErrorType.ResourceNotFound, localizer["UserNotFound"]);
        public static Error Exists(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
        public static Error ErrorWhenRegister(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
        public static Error CannotBeNullUserOrPassword(IStringLocalizer localizer) => new(ErrorType.ResourceNotFound, localizer["CannotBeNullUserOrPassword"]);
    }
}
