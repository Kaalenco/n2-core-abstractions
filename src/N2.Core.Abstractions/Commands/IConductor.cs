
namespace N2.Core.Commands;

/// <summary>
/// The conductor is used to find command handlers, start commands and accept response messages.
/// </summary>
public interface IConductor
{
    /// <summary>
    /// The 'IsConfigured' flag can be used to verify if the conductor is configured correctly
    /// and can accept messages.
    /// </summary>
    bool IsConfigured { get; }

    /// <summary>
    /// Registeren handlers
    /// </summary>
    int CallbackHandlerCount { get; }

    /// <summary>
    /// If active, the conductor is either initializing commands or there are some threads running.
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Validate is a command can be handled by any of the registered command handlers.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <returns></returns>
    bool IsCommandAvailable<TCommand>() where TCommand : ICommandRequest;

    /// <summary>
    /// Find command handler(s) and invoke the command.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="command"></param>
    ResponseStatus Invoke<TCommand>(TCommand command) where TCommand : ICommandRequest;

    /// <summary>
    /// Check registered callback methods and return results.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="result"></param>
    void CallBack<TResponse>(TResponse result) where TResponse : ICommandResponse;

    /// <summary>
    /// Register a callback function.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    ICallback RegisterCallback<TResponse>(Action<ICommandResponse> value) where TResponse : ICommandResponse;
}

/// <summary>
/// The callback interface is used to register a callback function.
/// </summary>
public interface ICallback : IDisposable
{
    Guid? Handle { get; }
}
