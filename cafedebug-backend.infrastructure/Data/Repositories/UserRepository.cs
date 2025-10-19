using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public class UserRepository(CafedebugContext context) : BaseRepository<UserAdmin>(context), IUserRepository
{
    private readonly CafedebugContext _context = context;
    public async Task<UserAdmin?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
    }
}