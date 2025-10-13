using N2.Core.Commands;

namespace N2.Core.Identity;

/// <summary>
/// Use the user manager to manage users in the identity system. It provides methods to create,
/// find, and manage users, including their roles and email addresses. The user manager is not
/// for validating user credentials and manage logon identities, but rather for managing user data and roles.
/// </summary>
/// <remarks>Use the <see cref="IIdentityManager"/> to validate user credentials and manage logon identities.</remarks>
/// <typeparam name="TUser"></typeparam>
public interface IUserManager<TUser> : IDisposable
    where TUser : IIdentityUser
{
    /// <summary>
    /// Check if the user manager supports the storing and retrieval of email addresses with the user data.
    /// </summary>
    bool SupportsUserEmail { get; }

    Task<bool> CanSignInAsync(TUser user, CancellationToken token);

    Task<ICommandResponse> ConfirmEmailAsync(TUser user, string confirmationToken, CancellationToken token);

    Task<ICommandResponse> CreateAsync(TUser user, string password, CancellationToken token);

    Task<ICommandResponse> CreateRoleAsync(string role, CancellationToken token);

    Task<ICommandResponse> DeleteAsync(TUser user, CancellationToken token);

    Task<ICommandResponse<TUser>> FindByEmailAsync(string emailAddress, CancellationToken token);
    Task<ICommandResponse<TUser>> FindByIdAsync(Guid userId, CancellationToken token);
    Task<ICommandResponse<TUser>> FindByNameAsync(string userName, CancellationToken token);

    Task<ICommandResponse<string>> GenerateEmailConfirmationTokenAsync(TUser user, CancellationToken token);

    Task<IListResponse<string>> GetRolesAsync(TUser user, CancellationToken token);

    Task<ICommandResponse<Guid>> GetUserIdAsync(TUser user, CancellationToken token);
    Task<ICommandResponse> IsInRoleAsync(TUser user, string role, CancellationToken token);
    Task<ICommandResponse> RemoveFromRoleAsync(TUser user, string role, CancellationToken token);
    Task<ICommandResponse> AddToRoleAsync(TUser user, string role, CancellationToken token);

    Task<ICommandResponse> RemoveRoleAsync(string role, CancellationToken token);

    Task<bool> RoleExistsAsync(string role, CancellationToken token);

    Task<ICommandResponse> SetEmailAsync(TUser user, string email, CancellationToken token);

    Task<ICommandResponse> SetUserNameAsync(TUser user, string userName, CancellationToken token);

    Task<ICommandResponse> ValidateAsync(TUser user, string password, CancellationToken token);

    Task<ICommandResponse> ValidateMultifactorAsync(TUser user, MultiFactorType mfaType, CancellationToken token);

    Task<ICommandResponse<MultiFactorProperties>> SetMultifactorAsync(TUser user, MultiFactorType mfaType, string mfaToken, CancellationToken token);
}

public class MultiFactorProperties
{
    public MultiFactorType MultiFactorType { get; set; }
    public byte[]? QrCode { get; set; }
    public Uri? Uri { get; set; }
}

public enum MultiFactorType
{
    None = 0,
    Totp = 1,
    Email = 2,
    Sms = 3,
}