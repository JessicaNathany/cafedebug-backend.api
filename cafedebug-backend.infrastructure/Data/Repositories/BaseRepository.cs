using System.Linq.Expressions;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using cafedebug_backend.domain.Shared.Repositories;
using cafedebug_backend.infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public abstract class BaseRepository<TEntity>(CafedebugContext context) : IBaseRepository<TEntity>
    where TEntity : Entity
{
    public async Task<int> CountAsync()
    {
        return await context.Set<TEntity>().CountAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return;

        context.Set<TEntity>().Remove(entity);
        await SaveAsync(entity);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        // entity 8 migrar  _dbSet.Where(c => x.Id == id).ExecuteDelete() 
        var query = context.Set<TEntity>();

        if (asNoTracking)
            return await query.AsNoTracking().ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByCodeAsync(Guid code)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Code == code);
    }

    public virtual async Task<TEntity?> GetByIdAsync(int id)
    {
        return await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task SaveAsync(TEntity entity)
    {
        await context.Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await SaveChangesAsync();
    }
    
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await context.Set<TEntity>().Where(expression).AsNoTracking().AnyAsync();
    }
}