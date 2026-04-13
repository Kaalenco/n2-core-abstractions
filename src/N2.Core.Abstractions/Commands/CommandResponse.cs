using System.Text.Json.Serialization;

namespace N2.Core.Commands;

public abstract class CommandResponse : ICommandResponse
{
    public ResponseStatus Status { get; protected set; } = ResponseStatus.Success;

    [JsonIgnore(Condition =
        JsonIgnoreCondition.WhenWritingNull |
        JsonIgnoreCondition.WhenWritingDefault)]
    public string? Message { get; protected set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TrackingId? Handle { get; protected set; }

    [JsonIgnore(Condition =
    JsonIgnoreCondition.WhenWritingNull |
    JsonIgnoreCondition.WhenWritingDefault)]
    public long? ExecutionTime { get; protected set; }

    public CommandResponse WithHandle(Guid handle)
    {
        Handle = handle;
        return this;
    }

    public CommandResponse WithExecutionTime(long executionTime)
    {
        ExecutionTime = executionTime;
        return this;
    }

    protected CommandResponse()
    {
        Status = ResponseStatus.Success;
        Message = null;
        Handle = null;
    }

    protected CommandResponse(int responseStatus, string message)
    {
        Status = (ResponseStatus)responseStatus;
        Message = message;
        Handle = null;
    }

    protected CommandResponse(int responseStatus, string message, Guid handle)
    {
        Status = (ResponseStatus)responseStatus;
        Message = message;
        Handle = handle;
    }

    protected CommandResponse(ResponseStatus responseStatus, string message, Guid handle)
    {
        Status = responseStatus;
        Message = message;
        Handle = handle;
    }

    protected CommandResponse(int responseStatus, string message, Guid handle, long executionTime)
    {
        Status = (ResponseStatus)responseStatus;
        Message = message;
        Handle = handle;
        ExecutionTime = executionTime;
    }

    protected CommandResponse(ResponseStatus responseStatus, string message, Guid handle, long executionTime)
    {
        Status = responseStatus;
        Message = message;
        Handle = handle;
        ExecutionTime = executionTime;
    }

    protected CommandResponse(ResponseStatus responseStatus, string message)
    {
        Status = responseStatus;
        Message = message;
        Handle = null;
    }

    protected CommandResponse(int responseStatus)
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

    public ICommandResponse CreateNew(ResponseStatus status, string? message = null, Guid? handle = null, long? executionTime = null)
    {
        if (Activator.CreateInstance(GetType()) is CommandResponse response)
        {
            response.Status = status;
            response.Message = message;
            response.Handle = handle;
            response.ExecutionTime = executionTime;
            return response;
        }
        throw new InvalidOperationException($"Cannot create instance of {GetType().FullName}.");
    }

    public object Clone()
    {
        if (Activator.CreateInstance(GetType()) is CommandResponse response)
        {
            response.Status = Status;
            response.Message = Message;
            response.Handle = Handle;
            response.ExecutionTime = ExecutionTime;
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
