using cafedebug_backend.domain.Enums;
using cafedebug_backend.domain.Errors;
using Microsoft.Extensions.Localization;

namespace cafedebug_backend.domain.Erros
{
    public class AuthenticationErros
    {
        public static Error EmailOrPasswordInvalid(IStringLocalizer localizer) => new(ErrorType.EmailOrPasswordInvalid, localizer["EmailOrPasswordInvalid"]);
    }
}
