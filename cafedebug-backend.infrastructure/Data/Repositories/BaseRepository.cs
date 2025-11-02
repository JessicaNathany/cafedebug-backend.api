using cafedebug_backend.domain.Shared;
using cafedebug_backend.domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using cafedebug_backend.infrastructure.Data.Pagination;

namespace cafedebug_backend.infrastructure.Data.Repositories;

public abstract class BaseRepository<TEntity>(CafedebugContext context) : IBaseRepository<TEntity>
    where TEntity : Entity
{
    public async Task<int> CountAsync()
    {
        return await context.Set<TEntity>().CountAsync();
    }

    public Task DeleteAsync(TEntity entity)
    {
        context.Set<TEntity>().Remove(entity);
        return SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            return;

        context.Set<TEntity>().Remove(entity);
        await SaveChangesAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false)
    {
        // entity 8 migrar  _dbSet.Where(c => x.Id == id).ExecuteDelete() 
        var query = context.Set<TEntity>();

        if (asNoTracking)
            return await query.AsNoTracking().ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<IPagedResult<TEntity>> GetPageList(int page = 1, int pageSize = 10, string? sortBy = null,
        bool descending = false, CancellationToken cancellationToken = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Prevent excessive page sizes
        
        var query = GetQuery(x => true)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(sortBy))
        {
            query = ApplySorting(query, sortBy, descending);
        }
        
        return await PagedList<TEntity>.CreateAsync(query, page, pageSize, sortBy, descending, cancellationToken);
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

    public IQueryable<TEntity> GetQueryable()
    {
        return context.Set<TEntity>();
    }

    private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>>? filterExpression)
    {
        var queryable = GetQueryable();

        return filterExpression is not null ? queryable.Where(filterExpression) : queryable;
    }

    private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, string sortBy, bool descending)
    {
        var allowedSortFields = GetAllowedSortFields();
        
        sortBy = sortBy.Trim();
    
        if (string.IsNullOrWhiteSpace(sortBy) || 
            !allowedSortFields.Contains(sortBy, StringComparer.OrdinalIgnoreCase))
            return query;
        
        var propertyInfo = typeof(TEntity).GetProperty(sortBy,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
            return query;

        return descending
            ? query.OrderByDescending(e => EF.Property<object>(e, propertyInfo.Name))
            : query.OrderBy(e => EF.Property<object>(e, propertyInfo.Name));
    }
    
    protected virtual HashSet<string> GetAllowedSortFields()
    {
        return typeof(TEntity).GetProperties()
            .Where(p => p.CanRead && p.GetGetMethod()?.IsPublic == true)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }
}