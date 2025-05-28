namespace N2.Core.Commands;

/// <summary>
/// The command request.
/// </summary>
public interface ICommandRequest
{
    /// <summary>
    /// Gets or sets the handle for the request.
    /// The handle should be used in events, response messages etc.
    /// to enable the calling process to link elements together
    /// and to simplify debugging and log analysis.
    /// </summary>
    string? Handle { get; }

    /// <summary>
    /// Gets or sets a value indicating that the command is
    /// validated only, but not processed.
    /// </summary>
    char? DoNotProcess { get; }
}

/// <summary>
/// The command request, extended with a generic content element.
/// </summary>
public interface ICommandRequest<T> : ICommandRequest
{
    /// <summary>
    /// Gets or sets the content for the command.
    /// </summary>
    T? Value { get; set; }
}
