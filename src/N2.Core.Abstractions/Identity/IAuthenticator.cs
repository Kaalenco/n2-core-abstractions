namespace N2.Core.Identity;

public interface IAuthenticator
{
    Task<IUserContext?> AuthenticateAsync(IUserLogin userLogin);
}
