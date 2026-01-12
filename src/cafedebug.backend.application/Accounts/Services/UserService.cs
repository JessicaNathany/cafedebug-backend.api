using cafedebug.backend.application.Accounts.DTOs.Requests;
using cafedebug.backend.application.Accounts.DTOs.Response;
using cafedebug.backend.application.Accounts.Interfaces;
using cafedebug.backend.application.Accounts.Validators;
using cafedebug.backend.application.Common.Mappings;
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

    public async Task<Result<UserAdminResponse>> CreateAsync(string email, string password)
    {
        if (String.IsNullOrEmpty(email))
            return Result.Failure<UserAdminResponse>(UserError.EmailCannotBeNull());

        var emailValidator = new EmailValidation();
        var emailValidationResult = emailValidator.Validate(email);

        var hashedPassword = passwordHasher.HashPassword(null, password);

        var user = new UserAdmin
        {
            Email = email,
            HashedPassword = hashedPassword,
            Code = new Guid(),
            CreatedDate = DateTime.Now
        };

        await userRepository.SaveAsync(user);

        var response = MappingConfig.ToUserAdmin(user);

        return Result.Success(response);
    }

    public async Task<Result<UserAdminResponse>> UpdateAsync(UserRequest userRequest)
    {
        var user = await userRepository.GetByEmailAsync(userRequest.Email);

        if (user is null)
            return Result.Failure<UserAdminResponse>(UserError.NotFound());

        await userRepository.UpdateAsync(user);

        var response = MappingConfig.ToUserAdmin(user);

        return Result.Success(response);
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
            return Result.Failure<UserAdminResponse>(UserError.NotFound());

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