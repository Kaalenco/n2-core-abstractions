
namespace N2.Core.Identity;

public interface IUserContext
{
    Guid PublicId { get; }

    string Name { get; }
    string Description { get; }
    string Email { get; }
    string? PhoneNumber { get; }
    string? ProfileImagePath { get; }
    string? ProfileThumbnailPath { get; }
    string? ProfileBackgroundImagePath { get; }

    void Alert(string message, Priority priority);
    IEnumerable<string> CurrentRoles();
    IEnumerable<UserAlert> Alerts { get; }

    bool IsAuthenticated { get; }
    bool CanPublish();
    bool CanModifyRights();
    bool CanDesign();
    bool IsAdmin() => CurrentRoles().Any(m => m == SystemRoles.Admin || m == SystemRoles.SuperAdmin);

    /// <summary>
    /// The primary partition key for the user. The partition
    /// key is used to read the user relevant data from a data repository.
    /// </summary>
    int PrimaryPartitionKey { get; }

    /// <summary>
    /// If the user is in the role, this will return true.
    /// </summary>
    /// <param name="role">a case insensitive role name.</param>
    /// <returns>boolean value.</returns>
    bool IsInRole(string role);

    /// <summary>
    /// If the user has the policy, this will return true.
    /// The policy can be requested using the PolicyValue method.
    /// </summary>
    /// <param name="policy">A name for the policy.</param>
    /// <returns>boolean value.</returns>
    bool HasPolicy(string policy);

    /// <summary>
    /// Get the value of a policy for the user. If the policy
    /// is not set, a PolicyNotFoundException will be thrown.
    /// </summary>
    /// <typeparam name="T">The expected response type.</typeparam>
    /// <param name="policy">The name for the policy</param>
    /// <returns>A non null struct class, containing the policy settings.</returns>
    T? PolicyValue<T>(string policy);
}
