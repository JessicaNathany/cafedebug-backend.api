using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Accounts.Validators;
using cafedebug.backend.application.Common.Mappings;
using cafedebug.backend.application.Common.Pagination;
using cafedebug_backend.domain.Accounts;
using cafedebug_backend.domain.Accounts.Errors;
using cafedebug_backend.domain.Interfaces.Repositories;
using cafedebug_backend.domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace cafedebug.backend.application.Accounts.Services;

/// <summary>
/// User admin service: class responsible for the business rules of the user admin to access admin area.
/// </summary>
public class UserService(IUserRepository userRepository, IPasswordHasher<UserAdmin> passwordHasher) : IUserService
{
    public async Task<Result<UserAdminResponse>> GetByLoginAndPasswordAsync(string email, string password)
    {
        var user = await userRepository.GetByEmailAsync(email);

        if (user is null)
            return Result.Failure<UserAdminResponse>(UserError.NotFound(email));

        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);

        if (verificationResult != PasswordVerificationResult.Success)
            return Result.Failure<UserAdminResponse>(UserError.InvalidPassword());

        var response = MappingConfig.ToUserAdmin(user);
        return Result.Success(response);
    }

    public async Task<Result<UserAdminResponse>> CreateAsync(UserAdminRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure<UserAdminResponse>(UserError.NameCannotBeNull());

        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Failure<UserAdminResponse>(UserError.EmailCannotBeNull());

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Failure<UserAdminResponse>(UserError.PasswordCannotBeNull());

        var emailValidationResult = new EmailValidation().Validate(request.Email);
        if (!emailValidationResult.IsValid)
            return Result.Failure<UserAdminResponse>(UserError.InvalidEmailFormat());

        var userAlreadyExists = await userRepository.AnyAsync(x => x.Email == request.Email);
        if (userAlreadyExists)
            return Result.Failure<UserAdminResponse>(UserError.UserAlreadyExists(request.Email));

        var hashedPassword = passwordHasher.HashPassword(null, request.Password);

        var user = new UserAdmin
        {
            Name = request.Name,
            Email = request.Email,
            HashedPassword = hashedPassword,
            CreatedAt = DateTime.Now
        };

        await userRepository.SaveAsync(user);

        var response = MappingConfig.ToUserAdmin(user);

        return Result.Success(response);
    }

    public async Task<Result<UserAdminResponse>> UpdateAsync(UserAdminRequest request, int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure<UserAdminResponse>(UserError.NotFound());

        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure<UserAdminResponse>(UserError.NameCannotBeNull());

        if (string.IsNullOrWhiteSpace(request.Email))
            return Result.Failure<UserAdminResponse>(UserError.EmailCannotBeNull());

        if (string.IsNullOrWhiteSpace(request.Password))
            return Result.Failure<UserAdminResponse>(UserError.PasswordCannotBeNull());

        var emailValidationResult = new EmailValidation().Validate(request.Email);
        if (!emailValidationResult.IsValid)
            return Result.Failure<UserAdminResponse>(UserError.InvalidEmailFormat());

        var emailInUse = await userRepository.AnyAsync(x => x.Email == request.Email && x.Id != id);
        if (emailInUse)
            return Result.Failure<UserAdminResponse>(UserError.UserAlreadyExists(request.Email));

        user.Name = request.Name;
        user.Email = request.Email;
        user.HashedPassword = passwordHasher.HashPassword(user, request.Password);
        user.UpdatedAt = DateTime.Now;

        await userRepository.UpdateAsync(user);

        var response = MappingConfig.ToUserAdmin(user);

        return Result.Success(response);
    }

    public async Task<Result<PagedResult<UserAdminResponse>>> GetAllAsync(PageRequest request)
    {
        var users = await userRepository.GetPageList(request.Page, request.PageSize, request.SortBy, request.Descending);

        return users.MapToPagedResult(user => user.ToUserAdmin());
    }

    public async Task<Result<UserAdminResponse>> GetByIdAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure<UserAdminResponse>(UserError.NotFound());

        var response = MappingConfig.ToUserAdmin(user); 

        return Result.Success(response);
    }

    public async Task<Result> DeleteAsync(int id)
    {
        var user = await userRepository.GetByIdAsync(id);

        if (user is null)
            return Result.Failure(UserError.NotFound());

        await userRepository.DeleteAsync(user.Id);
        return Result.Success();
    }

    public async Task<Result<UserAdminResponse>> GetUserAdminByEmail(string email)
    {
        var user = await userRepository.GetByEmailAsync(email);

        if (user is null)
            return Result.Failure<UserAdminResponse>(UserError.NotFound(email));

        var response = MappingConfig.ToUserAdmin(user);
        return Result.Success(response);
    }
}