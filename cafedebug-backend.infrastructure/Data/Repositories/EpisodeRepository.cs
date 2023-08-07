using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;

namespace cafedebug_backend.infrastructure.Data.Repository
{
    public class EpisodeRepository : Repository<Episode>, IEpisodeRepository
    {
        private readonly CafedebugContext _context;

        public EpisodeRepository(CafedebugContext context) : base(context)
        {
            _context = context;
        }
    }
}
