using N2.Core.Commands;

namespace N2.Core.Exceptions;

/// <summary>
/// A base abstraction for all N2 core exceptions.
/// </summary>
public class N2CoreException : Exception
{
    /// <summary>
    /// Gets the response status.
    /// </summary>
    public ResponseStatus ResponseStatus { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    public N2CoreException() : base(ResponseStatus.NotAccepted.ToMessage())
    {
        ResponseStatus = ResponseStatus.NotAccepted;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    public N2CoreException(string message) : base(message)
    {
        ResponseStatus = ResponseStatus.NotAccepted;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    /// <param name="status">
    /// The status.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    public N2CoreException(ResponseStatus status, string message) : base(message)
    {
        ResponseStatus = status;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    /// <param name="status">
    /// The status.
    /// </param>
    public N2CoreException(ResponseStatus status) : base(status.ToMessage())
    {
        ResponseStatus = status;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="innerException">
    /// The inner exception.
    /// </param>
    public N2CoreException(string? message, Exception? innerException) : base(message, innerException)
    {
        ResponseStatus = ResponseStatus.NotAccepted;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="N2CoreException" /> class.
    /// </summary>
    /// <param name="status">
    /// The response status.
    /// </param>
    /// <param name="message">
    /// The message.
    /// </param>
    /// <param name="innerException">
    /// The inner exception.
    /// </param>
    public N2CoreException(ResponseStatus status, string message, Exception? innerException) : base(message, innerException)
    {
        ResponseStatus = status;
    }

    /// <summary>
    /// Sets the response status for the exception.
    /// </summary>
    /// <param name="status">Any <see cref="ResponseStatus"/></param>
    /// <returns></returns>
    public N2CoreException UseStatus(ResponseStatus status)
    {
        ResponseStatus = status;
        return this;
    }
}
