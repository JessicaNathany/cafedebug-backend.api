﻿using cafedebug_backend.domain.Entities;
using cafedebug_backend.domain.Shared;

namespace cafedebug_backend.domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserAdmin>> GetByLoginAndPasswordAsync(string email, string password, CancellationToken cancellationToken);

        Task<Result<UserAdmin>> CreateAsync(string email, string password, CancellationToken cancellationToken);
    }
}
