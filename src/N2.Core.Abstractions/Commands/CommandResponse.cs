using System.Text.Json.Serialization;

namespace N2.Core.Commands;

public abstract class CommandResponse : ICommandResponse
{
    public ResponseStatus Status { get; protected set; }

    [JsonIgnore(Condition =
        JsonIgnoreCondition.WhenWritingNull |
        JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; protected set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Handle { get; protected set; }

    public CommandResponse WithHandle(string handle)
    {
        Handle = handle;
        return this;
    }

    public CommandResponse()
    {
        Status = ResponseStatus.Success;
        Message = null;
        Handle = null;
    }

    public CommandResponse(int responseStatus, string message)
    {
        Status = (ResponseStatus)responseStatus;
        Message = message;
        Handle = null;
    }

    public CommandResponse(int responseStatus, string message, string handle)
    {
        Status = (ResponseStatus)responseStatus;
        Message = message;
        Handle = handle;
    }

    public CommandResponse(ResponseStatus responseStatus, string message, string handle)
    {
        Status = responseStatus;
        Message = message;
        Handle = handle;
    }

    public CommandResponse(ResponseStatus responseStatus, string message)
    {
        Status = responseStatus;
        Message = message;
        Handle = null;
    }

    public CommandResponse(int responseStatus)
    {
        Status = (ResponseStatus)responseStatus;
        Message = null;
        Handle = null;
    }

    public bool Success => (int)Status <= 300;

    public virtual bool Equals(CommandResponse? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Status != Status)
        {
            return false;
        }

        if (other.Handle != Handle)
        {
            return false;
        }

        return true;
    }

    public override string ToString() => $"{Status}, {Message}";

    public static implicit operator int(CommandResponse r) => ToInt32(r);

    public static int ToInt32(CommandResponse r) => r != null ? (int)r.Status : 0;

    public override int GetHashCode() => Status.GetHashCode();

    public int ToInt()
    {
        return ToInt32(this);
    }

    public ICommandResponse CreateNew(ResponseStatus status, string? message = null, string? handle = null)
    {
        if (Activator.CreateInstance(GetType()) is CommandResponse response)
        {
            response.Status = status;
            response.Message = message;
            response.Handle = handle;
            return response;
        }
        throw new InvalidOperationException($"Cannot create instance of {GetType().FullName}.");
    }
}

/// <summary>
/// The command result.
/// </summary>
public abstract class CommandResponse<T> : CommandResponse, ICommandResponse<T>
{
    /// <summary>
    /// Gets the content.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public T? Value { get; set; }

}