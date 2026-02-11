using System.Linq.Expressions;

namespace cafedebug_backend.domain.Shared.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task SaveAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task DeleteAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

    Task<IPagedResult<TEntity>> GetPageList(int page = 1, int pageSize = 10,
        string? sortBy = null, bool descending = false,
        CancellationToken cancellationToken = default);
    
    IQueryable<TEntity> GetQueryable();
    
    Task<TEntity?> GetByIdAsync(int id);

    Task<int> CountAsync();

    Task<int> SaveChangesAsync();

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
}