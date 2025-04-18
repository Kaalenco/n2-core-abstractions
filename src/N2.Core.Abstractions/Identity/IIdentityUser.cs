namespace N2.Core.Identity;

public interface IIdentityUser
{
    Guid Id { get; set; }
    string? UserName { get; set; }
    string? NormalizedUserName { get; set; }
    string? Email { get; set; }
    string? NormalizedEmail { get; set; }
    bool EmailConfirmed { get; set; }
    string? DisplayName { get; set; }
    string? PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets a salted and hashed representation of the password for this user.
    /// </summary>
    string? PasswordHash { get; set; }

    /// <summary>
    /// A random value that must change whenever a users credentials change (password changed, login removed)
    /// </summary>
    string? SecurityStamp { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
    /// </summary>
    bool TwoFactorEnabled { get; set; }

    /// <summary>
    /// Gets or sets the date and time, in UTC, when any user lockout ends.
    /// </summary>
    /// <remarks>
    /// A value in the past means the user is not locked out.
    /// </remarks>
    DateTimeOffset? LockoutEnd { get; set; }

    /// <summary>
    /// Gets or sets a flag indicating if the user could be locked out.
    /// </summary>
    /// <value>True if the user could be locked out, otherwise false.</value>
    bool LockoutEnabled { get; set; }

    /// <summary>
    /// Gets or sets the number of failed login attempts for the current user.
    /// </summary>
    int AccessFailedCount { get; set; }
}
