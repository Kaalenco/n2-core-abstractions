namespace N2.Core.Identity;

public interface IAuthenticator
{
    Task<IUserContext?> Authenticate(IUserLogin userLogin, CancellationToken cancellationToken);
}
