using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.domain.Jwt;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class RefreshTokensRepository : BaseRepository<RefreshTokens>, IRefreshTokensRepository
    {
        public RefreshTokensRepository(CafedebugContext context) : base(context)
        {
        }

        public async Task<RefreshTokens> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
        }

        public async Task<RefreshTokens> GetByTokenByUserIdAsync(int userId)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
        }
    }
}
