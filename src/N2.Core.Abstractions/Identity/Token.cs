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
    public string? ClientSecret { get; set; }

    /// <summary>
    /// A refresh token that can be used to get a new access token. The refresh token can be
    /// used only once.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The UNIX epoch at which the token will expire.
    /// </summary>
    [JsonPropertyName("expires_in")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ExpiresIn { get; set; }

    /// <summary>
    /// The type of token.
    /// </summary>
    [JsonPropertyName("token_type")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? TokenType { get; set; }

    /// <summary>
    /// Contains a reference for the grant type.
    /// </summary>
    [JsonPropertyName("grant_type")]
    [JsonRequired]
    public string GrantType { get; set; } = "none";

    /// <summary>
    /// The scope for the token.
    /// </summary>
    [JsonPropertyName("scope")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Scope { get; set; }

    /// <summary>
    /// Serializes the token to a JSON string representation.
    /// </summary>
    /// <returns></returns>
    public string Serialize()
    {
        return System.Text.Json.JsonSerializer.Serialize(this);
    }
}
