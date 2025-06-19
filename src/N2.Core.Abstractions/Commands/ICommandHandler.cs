namespace N2.Core.Commands;

/// <summary>
/// The command handler abstraction.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Gets a value indicating whether a processing error occured.
    /// </summary>
    bool ProcessingError { get; }

    /// <summary>
    /// Gets or sets the timeout in milli seconds for a command process.
    /// </summary>
    int TimeoutInMilliSeconds { get; set; }

    /// <summary>
    /// Indication that the commandhandler is currently active and should not be disposed or removed from the conductor.
    /// Use this to indicate that the commandhandler is currently processing requests or waiting for responses.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Indication that the commandhandler is enabled and can handle requests.
    /// </summary>
    bool IsEnabled { get; }

    /// <summary>
    /// Indication that the commandhandler is configured and can handle requests of a specific type.
    /// </summary>
    /// <param name="command">A command that will be evaluated.</param>
    /// <returns>true if this command handler can handle the command.</returns>
    bool CanHandle(ICommandRequest command);

    /// <summary>
    /// Start the processing to handle the requests.
    /// </summary>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Invoke(ICommandRequest command);
}

/// <summary>
/// A command handler abstraction without a return value.
/// </summary>
public interface ICommandHandler<TQ> : ICommandHandler
     where TQ : ICommandRequest
{
    /// <summary>
    /// Accepts the request for processing.
    /// </summary>
    /// <param name="request">
    /// The request.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus ExecuteCommand(TQ request);
}

/// <summary>
/// A command handler abstraction with a return value.
/// </summary>
public interface ICommandHandler<TQ, TA> : ICommandHandler<TQ>
     where TQ : ICommandRequest
     where TA : class, ICommandResponse, new()
{
    /// <summary>
    /// Find the next response.
    /// </summary>
    /// <param name="peek">
    /// If true, the response will not be removed from the commandhandler.
    /// </param>
    /// <returns>
    /// A response.
    /// </returns>
    TA Response(bool peek);

    /// <summary>
    /// Handle the request and wait for the response.
    /// </summary>
    /// <param name="request">
    /// The request.
    /// </param>
    /// <returns>
    /// </returns>
    Task<TA> WaitForAsync(TQ request);
}

/// <summary>
/// The queued command handler abstraction.
/// </summary>
public interface IQueuedCommandHandler<TQ> : ICommandHandler<TQ>
    where TQ : ICommandRequest
{
    /// <summary>
    /// Cancels the request with the provided handle.
    /// </summary>
    /// <param name="handle">
    /// The handle.
    /// </param>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Cancel(string handle);

    /// <summary>
    /// Counts the number of current requests.
    /// </summary>
    /// <returns>
    /// An int.
    /// </returns>
    int CountRequests();

    /// <summary>
    /// Counts the number of results.
    /// </summary>
    /// <returns>
    /// An int.
    /// </returns>
    int CountResults();
}