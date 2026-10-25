using Microsoft.AspNetCore.Identity;
using money_manager.Entities;
using money_manager.Repositories;

namespace money_manager.Services;

public class UserStore(IUserRepository userRepository) : IUserStore<UserEntity>, IUserEmailStore<UserEntity>, IUserPasswordStore<UserEntity>
{
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<IdentityResult> CreateAsync(UserEntity user, CancellationToken cancellationToken)
    {
        user.UserId = await _userRepository.CreateUserAsync(user);
        return IdentityResult.Success;
    }

    public Task<IdentityResult> DeleteAsync(UserEntity user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }

    public async Task<UserEntity?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        return await _userRepository.FindUserByEmailAsync(normalizedEmail);
    }

    public Task<UserEntity?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UserEntity?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _userRepository.FindUserByUserNameAsync(normalizedUserName);
    }

    public Task<string?> GetEmailAsync(UserEntity user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(UserEntity user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedEmailAsync(UserEntity user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetNormalizedUserNameAsync(UserEntity user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<string?> GetPasswordHashAsync(UserEntity user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.PasswordHash);
    }

    public Task<string> GetUserIdAsync(UserEntity user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string>(user.UserId.ToString());
    }

    public Task<string?> GetUserNameAsync(UserEntity user, CancellationToken cancellationToken)
    {
        return Task.FromResult<string?>(user.UserAccountName);
    }

    public Task<bool> HasPasswordAsync(UserEntity user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailAsync(UserEntity user, string? email, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetEmailConfirmedAsync(UserEntity user, bool confirmed, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetNormalizedEmailAsync(UserEntity user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail ?? string.Empty;
        return Task.CompletedTask;
    }

    public Task SetNormalizedUserNameAsync(UserEntity user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserAccountName = normalizedName ?? string.Empty;
        return Task.CompletedTask;
    }

    public Task SetPasswordHashAsync(UserEntity user, string? passwordHash, CancellationToken cancellationToken)
    {
        user.PasswordHash = passwordHash ?? string.Empty;
        return Task.CompletedTask;
    }

    public Task SetUserNameAsync(UserEntity user, string? userName, CancellationToken cancellationToken)
    {
        user.UserAccountName = userName ?? string.Empty;
        return Task.CompletedTask;
    }

    public async Task<IdentityResult> UpdateAsync(UserEntity user, CancellationToken cancellationToken)
    {
        await _userRepository.UpdatePasswordUserAsync(user);
        return IdentityResult.Success;
    }
}
