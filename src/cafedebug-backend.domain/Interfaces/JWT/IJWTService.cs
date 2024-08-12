using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.JWT
{
    public interface IJWTService
    {
        JWTToken GenerateToken(UserAdmin userAdmin);

        Task<Result<RefreshTokens>> GetByTokenAsync(string token);
    }
}
