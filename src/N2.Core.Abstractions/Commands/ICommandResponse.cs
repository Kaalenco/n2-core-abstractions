namespace N2.Core.Commands;

/// <summary>
/// The command result.
/// </summary>
public interface ICommandResponse
{
    /// <summary>
    /// Gets the handle. This handle should be the same as the
    /// handle of the command that initiated this result
    /// </summary>
    string? Handle { get; }

    /// <summary>
    /// The result <seealso cref="ResponseStatus"/>.
    /// </summary>
    ResponseStatus Status { get; }

    /// <summary>
    /// Gets the optional response message.
    /// </summary>
    string? Message { get; }
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