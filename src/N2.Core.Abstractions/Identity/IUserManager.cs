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

    Task<bool> CanSignIn(TUser user, CancellationToken token);

    Task<ICommandResponse> ConfirmEmail(TUser user, string confirmationToken, CancellationToken token);

    Task<ICommandResponse> Create(TUser user, string password, CancellationToken token);

    Task<ICommandResponse> CreateRole(string role, CancellationToken token);

    Task<ICommandResponse> Delete(TUser user, CancellationToken token);

    Task<ICommandResponse<TUser>> FindByEmail(string emailAddress, CancellationToken token);
    Task<ICommandResponse<TUser>> FindById(Guid userId, CancellationToken token);
    Task<ICommandResponse<TUser>> FindByName(string userName, CancellationToken token);

    Task<ICommandResponse<string>> GenerateConfirmationToken(TUser user, CancellationToken token);

    Task<IListResponse<string>> GetRoles(TUser user, CancellationToken token);

    Task<ICommandResponse<Guid>> GetUserId(TUser user, CancellationToken token);
    Task<ICommandResponse> IsInRole(TUser user, string role, CancellationToken token);
    Task<ICommandResponse> RemoveFromRole(TUser user, string role, CancellationToken token);
    Task<ICommandResponse> AddToRole(TUser user, string role, CancellationToken token);

    Task<ICommandResponse> RemoveRole(string role, CancellationToken token);

    Task<bool> RoleExists(string role, CancellationToken token);

    Task<ICommandResponse> SetEmail(TUser user, string email, CancellationToken token);

    Task<ICommandResponse> SetUserName(TUser user, string userName, CancellationToken token);

    Task<ICommandResponse> Validate(TUser user, string password, CancellationToken token);

    Task<ICommandResponse> ValidateMultifactor(TUser user, string multifactorCode, CancellationToken token);

    Task<ICommandResponse<MultiFactorProperties>> SetMultifactor(TUser user, MultiFactorType mfaType, string mfaToken, CancellationToken token);
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