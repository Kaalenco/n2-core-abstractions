namespace N2.Core.Identity;

public interface IWebTokenGenerator
{
    string GenerateWebToken(IUserContext userContext, int timeoutInMinutes);

    /// <summary>
    /// Generates a cryptographically random, opaque refresh token (88-character Base64 string).
    /// The caller is responsible for persisting it and associating it with the user.
    /// </summary>
    string GenerateRefreshToken();
}