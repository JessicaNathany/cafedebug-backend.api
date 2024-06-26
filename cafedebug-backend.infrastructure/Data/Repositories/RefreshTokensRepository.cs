using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.infrastructure.Context;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class RefreshTokensRepository : BaseRepository<RefreshTokens>, IRefreshTokensRepository
    {
        public RefreshTokensRepository(CafedebugContext context) : base(context)
        {
        }
    }
}
