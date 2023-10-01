using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respository;
using cafedebug_backend.infrastructure.Context;
using cafedebug_backend.infrastructure.Data.Repository;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories
{
    public class TagRepository : BaseRepository<Tag>, ITagRepository
    {
        public TagRepository(CafedebugContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Tag>> GetByEpisodeId(int id)
        {
            return _context.TagsEpisodes
               .Include(t => t.Tag)
               .Where(x => x.EpisodeId == id)
               .Select(t => new Tag { Name = t.Tag.Name, Id = t.TagId })
               .ToList();
        }

        public async Task<IEnumerable<Tag>> GetByName(string name)
        {
            return _context.Tags.Where(x => x.Name.Contains(name)).ToList();
        }

        public async Task<IEnumerable<Tag>> GetPagedAsync(string searchParam, int pageIndex = 0, int pageSize = 10)
        {
            var query = _context.Set<Tag>()
                .AsNoTracking()
                .Where(category => category.Name.Contains(searchParam))
                .Skip(pageIndex * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
    }
}
