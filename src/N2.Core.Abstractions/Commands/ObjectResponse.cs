
namespace N2.Core.Commands;

public class ObjectResponse<T> : CommandResponse, ICommandResponse<T> where T : new()
{
    public T Value { get; }

    public ObjectResponse() : base(500, string.Empty)
    {
        Value = new();
    }

    public ObjectResponse(T value, int responseStatus, string Message) : base(responseStatus, Message)
    {
        Value = value;
    }

    public ObjectResponse(T value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public bool Equals(ICommandResponse<T>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Status != Status)
        {
            return false;
        }

        return true;
    }

    public ObjectResponse<T> Fail(string message) => new(new(), 500, message);

    public ObjectResponse<T> Accept(T Value) => new(Value, 200);

    public override int GetHashCode() => HashCode.Combine(Status.GetHashCode(), Value?.GetHashCode());

    public void Deconstruct(out T Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}
