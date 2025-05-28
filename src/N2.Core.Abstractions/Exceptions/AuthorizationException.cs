using N2.Core.Commands;

namespace N2.Core.Exceptions;

/// <summary>
/// An authorization exception.
/// </summary>
public class AuthorizationException : N2CoreException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    public AuthorizationException() : base(ResponseStatus.Unauthorized)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public AuthorizationException(string message) : base(ResponseStatus.Unauthorized, message)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizationException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="e">The exception.</param>
    public AuthorizationException(string message, Exception e) : base(ResponseStatus.Unauthorized, message, e)
    {
    }
}