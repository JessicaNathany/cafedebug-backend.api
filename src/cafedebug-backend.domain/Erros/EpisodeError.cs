using cafedebug_backend.domain.Enums;
using cafedebug_backend.domain.Errors;
using Microsoft.Extensions.Localization;

namespace cafedebug_backend.domain.Erros
{
    public class EpisodeError
    {
        public static Error NotFound(IStringLocalizer localizer) => new(ErrorType.ResourceNotFound, localizer["EpisodeNotFoundError"]);
        public static Error Exists(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
        public static Error ErrorWhenRegister(IStringLocalizer localizer) => new(ErrorType.ExistingRegister, localizer["ExistingRegister"]);
    }
}
