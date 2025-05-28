namespace N2.Core.Identity;

public class TokenRequest : Commands.CommandRequest<Token>
{
    /// <summary>
    /// Gets or sets the audience for a token.
    /// </summary>
    public string Audience { get; set; } = string.Empty;
}
