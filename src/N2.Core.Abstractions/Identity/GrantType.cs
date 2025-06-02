namespace N2.Core.Identity;

public static class GrantType
{
    /// <summary>
    /// The client credentials grant type.
    /// </summary>
    public const string ClientCredentials = "client_credentials";
    /// <summary>
    /// The password grant type.
    /// </summary>
    public const string Password = "password";
    /// <summary>
    /// The authorization code grant type.
    /// </summary>
    public const string AuthorizationCode = "authorization_code";
    /// <summary>
    /// The refresh token grant type.
    /// </summary>
    public const string RefreshToken = "refresh_token";
    public const string Basic = "basic";
    public const string AccessToken = "access_token";
    public const string Refresh = "refresh";
    public const string LogoffUser = "logoff_user";
    public const string Logoff = "logoff";
}