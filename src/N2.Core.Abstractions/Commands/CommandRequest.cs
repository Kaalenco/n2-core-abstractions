using System.Text.Json.Serialization;

namespace N2.Core.Commands;

/// <summary>
/// The command request.
/// </summary>
/// <seealso cref="ICommandRequest" />
public abstract class CommandRequest : ICommandRequest
{
    /// <summary>
    /// Gets or sets the handle.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
    public string? Handle { get; set; }

    /// <summary>
    /// Gets or sets a value indicating that the command should be validated and not processed.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
    public char? DoNotProcess { get; set; }
}

/// <summary>
/// The command request.
/// </summary>
/// <seealso cref="ICommandRequest" />
public abstract class CommandRequest<T> : CommandRequest, ICommandRequest<T>
{
    /// <summary>
    /// Gets or sets the content for the command.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault | JsonIgnoreCondition.WhenWritingNull)]
    public T? Value { get; set; }
}