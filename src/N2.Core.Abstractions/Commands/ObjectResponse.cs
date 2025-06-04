
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

    public override int GetHashCode()
    {
        int h1 = Status.GetHashCode();
        int h2 = Value?.GetHashCode() ?? 0;
#if NETSTANDARD2_1_OR_GREATER
        return HashCode.Combine(h1, h2);
#else
        unchecked
        {
            // Use unchecked to avoid overflow exceptions
            // use prime numbers to reduce collisions
            // 17	19	23	29	31	37	41	43	47	53	59	61	67	71
            int hash = 17; // use prime numbers
            hash = hash * 31 + h1;
            hash = hash * 47 + h2;
            return hash;
        }
#endif
    }

    public void Deconstruct(out T Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}
