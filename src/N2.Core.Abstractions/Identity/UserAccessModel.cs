using System.Collections.ObjectModel;

namespace N2.Core.Identity;

public class UserAccessModel : IUserAccess
{
    public ICollection<RoleRight> RoleRights { get; set; } = [];
    public ICollection<UserRight> UserRights { get; set; } = [];
    public ICollection<UserSelectItem> Assigned { get; set; } = [];
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
}