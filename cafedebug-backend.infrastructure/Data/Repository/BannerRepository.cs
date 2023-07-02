using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;

namespace cafedebug_backend.infrastructure.Data.Repository
{
    public class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private readonly CafedebugContext _context;

        public BannerRepository(CafedebugContext context) : base(context)
        {
            _context = context;
        }
    }
}
