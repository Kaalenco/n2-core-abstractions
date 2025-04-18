
namespace N2.Core.Identity;

public interface IUserAccess
{
    Guid OwnerId { get; set; }
    string OwnerName { get; set; }
    ICollection<RoleRight> RoleRights { get; }
    ICollection<UserRight> UserRights { get; }
    ICollection<UserSelectItem> Assigned { get; }
}
