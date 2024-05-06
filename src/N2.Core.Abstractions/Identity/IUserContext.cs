
namespace N2.Identity;

public interface IUserContext
{
    Guid UserId { get; }
    string UserName { get; }
    string UserDescription { get; }
    string UserPhone { get; }
    string UserEmail { get; }
    void Alert(string message, Priority priority);
    IEnumerable<string> CurrentRoles();
    IEnumerable<UserAlert> Alerts { get; }

    bool IsAuthenticated { get; }
    bool CanPublish();
    bool CanModifyRights();
    bool CanDesign();
    bool IsAdmin() => CurrentRoles().Any(m => m == "Admin" || m == "SuperAdmin");
}
