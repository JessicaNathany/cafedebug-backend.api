using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository<UserAdmin>, IUserRepository
    {
        public UserRepository(CafedebugContext context) : base(context)
        {
        }

        public async Task<UserAdmin> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
