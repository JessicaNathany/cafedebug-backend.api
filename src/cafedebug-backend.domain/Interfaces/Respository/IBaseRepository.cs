﻿using cafedebug_backend.domain.Entities;
using System.Linq.Expressions;

namespace cafedebug_backend.domain.Interfaces.Respositories
{
    public interface IBaseRepository<TEntity> : IDisposable where TEntity : Entity
    {
        Task<TEntity> SaveAsync(TEntity entity);

        Task UpdateAsync(TEntity entity);

        Task DeleteAsync(Guid code);

        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        Task<TEntity> GetByIdAsync(int id);

        Task<TEntity> GetByCodeAsync(Guid code);

        Task<int> CountAsync();

        Task<int> SaveChangesAsync();
    }
}
