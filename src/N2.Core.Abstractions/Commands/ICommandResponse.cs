namespace N2.Core.Commands;

/// <summary>
/// The command result.
/// </summary>
public interface ICommandResponse
{
    /// <summary>
    /// Gets the handle. This handle should be the same as the handle of the command that initiated
    /// this result
    /// </summary>
    TrackingId? Handle { get; }

    /// <summary>
    /// The result <seealso cref="ResponseStatus" />.
    /// </summary>
    ResponseStatus Status { get; }

    /// <summary>
    /// Gets the optional response message.
    /// </summary>
    string? Message { get; }

    /// <summary>
    /// Gets the execution time in milliseconds for the command response.
    /// </summary>
    long? ExecutionTime { get; }

    /// <summary>
    /// Initializes the command response with a status, optional message and handle.
    /// </summary>
    ICommandResponse CreateNew(ResponseStatus status, string? message = null, Guid? handle = null, long? executionTime = null);
}

/// <summary>
/// The command result.
/// </summary>
public interface ICommandResponse<T> : ICommandResponse
{
    /// <summary>
    /// Gets the for the command result.
    /// </summary>
    T? Value { get; }
}