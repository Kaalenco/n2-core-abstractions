using System.Security.Claims;

namespace N2.Core.Identity;

/// <summary>
/// This interface defines the methods for managing user identities, including logon, logoff, token
/// refresh, and claim management. It is scoped to the identity management system and is used to
/// handle user sessions and claims.
/// </summary>
public interface IIdentityManager
{
    /// <summary>
    /// Find the refresh token and create a new refresh token.
    /// </summary>
    /// <param name="refresh">
    /// The refresh token.
    /// </param>
    /// <returns>
    /// </returns>
    Task<Guid> RefreshUserToken(Guid refresh);

    /// <summary>
    /// Logon with a user name and password. The password is validated by the authentication service.
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<Guid> LogonUser(string userName, string password);

    /// <summary>
    /// Remove all sessions for the user in the specified scope.
    /// </summary>
    /// <param name="refresh">
    /// The refresh token.
    /// </param>
    /// <param name="scope">
    /// The scope for the logoff.
    /// </param>
    /// <returns>
    /// </returns>
    Task LogoffUser(Guid refresh, string scope);

    /// <summary>
    /// Register a refresh token for the user.
    /// </summary>
    /// <param name="audience">
    /// The audience for the claims
    /// </param>
    /// <param name="sid">
    /// The user identifier
    /// </param>
    /// <param name="scope">
    /// The scope for the claimset
    /// </param>
    /// <param name="refreshToken">
    /// A refresh token that will be registered to enable token refresh.
    /// </param>
    /// <param name="tokenTimeout">
    /// The date and time for the token to lose its validation.
    /// </param>
    /// <returns>
    /// </returns>
    Task RegisterRefreshToken(Guid sid, string audience, string scope, Guid refreshToken, DateTime tokenTimeout);

    /// <summary>
    /// Find the claims in the identity repository
    /// </summary>
    /// <param name="audience">
    /// The audience for the claims
    /// </param>
    /// <param name="sid">
    /// The user identifier
    /// </param>
    /// <param name="scope">
    /// The scope for the claimset
    /// </param>
    /// <returns>
    /// A list of claims
    /// </returns>
    Task<Claim[]> GetClaims(Guid sid, string audience, string scope);

    /// <summary>
    /// Logon with a user name and a validated secret. The secret is validated by the authentication service.
    /// </summary>
    /// <param name="userName">
    /// </param>
    /// <param name="secret">
    /// </param>
    /// <returns>
    /// </returns>
    Task<Guid> LogonUserWithSecret(string userName, string secret);

    /// <summary>
    /// Retrieve the user secret for the specified scope.
    /// </summary>
    /// <param name="userName">
    /// </param>
    /// <param name="scope">
    /// </param>
    /// <returns>
    /// </returns>
    Task<string> GetUserSecret(string userName, string scope);

    /// <summary>
    /// Get the current user identifier.
    /// </summary>
    string Currentuser { get; }

    /// <summary>
    /// Returns true if the current user is authorized to access the system.
    /// </summary>
    bool Authorized { get; }

    /// <summary>
    /// Return a JSON Web Token (JWT) for the current user session.
    /// </summary>
    /// <returns>A json web token</returns>
    string JWT();
}