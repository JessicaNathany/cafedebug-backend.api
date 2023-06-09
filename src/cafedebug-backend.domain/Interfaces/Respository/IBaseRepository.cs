using cafedebug_backend.domain.Entities;
using System.Linq.Expressions;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task UpdateAsyn(TEntity entity);

        Task DeleteAsync(Guid code);

        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByCodeAsync(Guid code);

        Task<int> CountAsync();

        Task<int> SaveChangesAsync();
    }
}
