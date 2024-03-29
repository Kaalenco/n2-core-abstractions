
namespace N2.Core;

public class ApplicationSetup
{
    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Logo { get; set; } = string.Empty;
    public string AdminEmail { get; set; } = string.Empty;
    public string SupportEmail { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;
    public string[] Roles { get; set; } = [];
}