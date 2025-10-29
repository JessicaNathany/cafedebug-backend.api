using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Repositories;
using cafedebug_backend.domain.Accounts.Tokens;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Jwt;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class RefreshTokensRepository(CafedebugContext context) : BaseRepository<RefreshTokens>(context), IRefreshTokensRepository
{
    private readonly CafedebugContext _context = context;

    public async Task<RefreshTokens?> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == token);
    }

    public async Task<RefreshTokens?> GetByTokenByUserIdAsync(int userId)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}