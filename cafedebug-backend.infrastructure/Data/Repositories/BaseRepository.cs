using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Interfaces.Respositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repository
{
    public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : Entity
    {
        protected readonly CafedebugContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseRepository(CafedebugContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public async Task<int> CountAsync()
        {
            return await _dbSet.CountAsync();
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

            _dbSet.Remove(entity);
            await SaveAsync(entity, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
        {
            var query = _context.Set<TEntity>();

            if (asNoTracking)
               return await query.AsNoTracking().ToListAsync();

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetByCodeAsync(Guid code, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Code == code);
        }

        public virtual async Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<TEntity> SaveAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _dbSet.Add(entity);
            await SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await SaveAsync(entity, cancellationToken);
        }
    }
}
