namespace N2.Core.Identity;

public interface IUserManager<TUser> : IDisposable
where TUser : IIdentityUser
{
    /// <summary>
    /// Check if the user manager supports the storing and retrieval of email addresses with the user data.
    /// </summary>
    bool SupportsUserEmail { get; }

    Task<bool> CanSignInAsync(TUser user, CancellationToken token);

    Task<IRequestResult> ConfirmEmailAsync(TUser user, string confirmationToken, CancellationToken token);

    Task<IRequestResult> CreateAsync(TUser user, string password, CancellationToken token);

    Task<IRequestResult> CreateRoleAsync(string role, CancellationToken token);

    Task<IRequestResult> DeleteAsync(TUser user, CancellationToken token);

    Task<TUser?> FindByEmailAsync(string emailAddress, CancellationToken token);
    Task<TUser?> FindByIdAsync(Guid userId, CancellationToken token);
    Task<TUser?> FindByNameAsync(string userName, CancellationToken token);

    Task<string> GenerateEmailConfirmationTokenAsync(TUser user, CancellationToken token);

    Task<IList<string>> GetRolesAsync(TUser user, CancellationToken token);

    Task<Guid> GetUserIdAsync(TUser user, CancellationToken token);
    Task<bool> IsInRoleAsync(TUser user, string role, CancellationToken token);
    Task<IRequestResult> RemoveFromRoleAsync(TUser user, string role, CancellationToken token);
    Task<IRequestResult> AddToRoleAsync(TUser user, string role, CancellationToken token);

    Task<IRequestResult> RemoveRoleAsync(string role, CancellationToken token);

    Task<bool> RoleExistsAsync(string role, CancellationToken token);

    Task<IRequestResult> SetEmailAsync(TUser user, string email, CancellationToken token);

    Task<IRequestResult> SetUserNameAsync(TUser user, string userName, CancellationToken token);
    Task<IRequestResult> ValidateAsync(TUser user, string password, CancellationToken token);
}