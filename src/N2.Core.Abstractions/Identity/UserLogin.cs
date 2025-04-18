namespace N2.Core.Identity;

public class UserLogin : IUserLogin
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
