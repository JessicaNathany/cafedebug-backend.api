using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;

namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        Task<JWTToken> GenerateToken(UserAdmin userAdmin);
    }
}
