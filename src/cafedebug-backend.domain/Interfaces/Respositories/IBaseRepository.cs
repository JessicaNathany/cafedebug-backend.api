﻿using cafedebug_backend.domain.Entities;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IBaseRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(int id, CancellationToken cancellationToken);

        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<TEntity> GetByCodeAsync(Guid code, CancellationToken cancellationToken);

        Task<int> CountAsync();

        Task<int> SaveChangesAsync();
    }
}
