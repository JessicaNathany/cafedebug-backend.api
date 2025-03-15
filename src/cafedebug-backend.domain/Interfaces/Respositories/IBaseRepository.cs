using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByCodeAsync(Guid code);

        Task<int> CountAsync();

        Task<int> SaveChangesAsync();
    }
}
