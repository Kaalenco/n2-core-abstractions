using N2.Core.Commands;

namespace N2.Core.Identity;

public class TokenResponse : CommandResponse<Token>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TokenResponse"/> class.
    /// </summary>
    public TokenResponse()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenResponse"/> class.
    /// </summary>
    /// <param name="value">The (new) token.</param>
    public TokenResponse(Token value)
    {
        Value = value;
        Status = Commands.ResponseStatus.Success;
    }

    public static TokenResponse Failed(ResponseStatus status, string message)
    {
        return new TokenResponse
        {
            Status = status,
            Message = message
        };
    }
}