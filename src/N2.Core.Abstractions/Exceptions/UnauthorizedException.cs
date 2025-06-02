using N2.Core.Commands;

namespace N2.Core.Exceptions;

/// <summary>
/// An authorization exception.
/// </summary>
public class UnauthorizedException : N2CoreException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    public UnauthorizedException() : base(ResponseStatus.Unauthorized)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    public UnauthorizedException(string message) : base(ResponseStatus.Unauthorized, message)
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UnauthorizedException"/> class.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="e">The exception.</param>
    public UnauthorizedException(string message, Exception e) : base(ResponseStatus.Unauthorized, message, e)
    {
    }
}