using System.Linq.Expressions;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : Entity
{
    Task SaveAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);

    Task DeleteAsync(int id);

    Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

    Task<TEntity?> GetByIdAsync(int id);

    Task<TEntity?> GetByCodeAsync(Guid code);

    Task<int> CountAsync();

    Task<int> SaveChangesAsync();

    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
}