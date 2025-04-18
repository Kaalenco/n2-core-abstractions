namespace N2.Core.Identity;

public class UserProfile : UserLogin
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public string? DisplayName { get; set; }
}