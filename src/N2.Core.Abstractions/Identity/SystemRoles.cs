namespace N2.Core.Identity;

/// <summary>
/// The predefined roles in the application.
/// </summary>
/// <remarks>
/// A role name cannot exceed 11 characters.
/// 'Application' contains 11 characters.
/// </remarks>
public static class SystemRoles
{
    public const string Admin = "admin";
    public const string SuperAdmin = "superadmin";
    public const string Application = "application";
    public const string AuthManager = "authmanager";
    public const string Designer = "designer";
    public const string Publisher = "publisher";
    public const string SysAdmin = "sysadmin";
    public const string User = "user";
    public const string Visitor = "visitor";
}
