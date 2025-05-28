using System.Text.Json.Serialization;

namespace N2.Core.Identity;

/// <summary>
/// A token
/// </summary>
public class Token
{
    /// <summary>
    /// The access token string.
    /// </summary>
    [JsonPropertyName("access_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AccessToken { get; set; }

    /// <summary>
    /// Gets or sets the client_id.
    /// </summary>
    [JsonPropertyName("client_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client_secret.
    /// </summary>
    [JsonPropertyName("client_secret")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? client_secret { get; set; }

    /// <summary>
    /// A refresh token that can be used to get a new access token. The refresh token can be
    /// used only once.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? refresh_token { get; set; }

    /// <summary>
    /// The UNIX epoch at which the token will expire.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long? expires_in { get; set; }

    /// <summary>
    /// The type of token.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string? token_type { get; set; }

    /// <summary>
    /// Contains a reference for the grant type.
    /// </summary>
    [JsonPropertyName("grant_type")]
    [JsonRequired]
    public string GrantType { get; set; } = "basic";

    /// <summary>
    /// The scope for the token.
    /// </summary>
    [JsonPropertyName("scope")]
    public string? scope { get; set; }
}

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