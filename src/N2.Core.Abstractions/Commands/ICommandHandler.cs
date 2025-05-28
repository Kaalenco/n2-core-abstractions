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
    /// Start the processing to handle the requests.
    /// </summary>
    /// <returns>
    /// A ResponseStatus.
    /// </returns>
    ResponseStatus Invoke();
}

/// <summary>
/// The command handler abstraction.
/// </summary>
public interface ICommandHandler<TQ, TA> : ICommandHandler
    where TQ : class, ICommandRequest
    where TA : class, ICommandResponse, new()
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
    ResponseStatus Accept(TQ request);

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
public interface IQueuedCommandHandler<TQ, TA> : ICommandHandler<TQ, TA>
where TQ : class, ICommandRequest
where TA : class, ICommandResponse, new()
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