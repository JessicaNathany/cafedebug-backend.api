using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(CafedebugContext context) : base(context)
        {
        }

        public Task<User> GetByLoginAndPassword(string login, string senha)
        {
            throw new NotImplementedException();
        }
    }
}
