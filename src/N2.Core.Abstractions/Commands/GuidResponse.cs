namespace N2.Core.Commands;

public class GuidResponse : CommandResponse, ICommandResponse<Guid>
{
    public Guid Value { get; }

    public GuidResponse(Guid value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public GuidResponse(Guid value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public static GuidResponse Fail(Guid Value, string message) => new(Value, 500, message);

    public static GuidResponse Accept(Guid Value) => new(Value, 200);

    public static GuidResponse Created(Guid Value) => new(Value, 201);

    public static GuidResponse NoChanges(Guid Value, string message) => new(Value, 204, message);

    public virtual bool Equals(ICommandResponse<Guid>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Value != Value)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
#if NETSTANDARD2_1_OR_GREATER
        return HashCode.Combine(Status.GetHashCode(), Value.GetHashCode());
#else
        unchecked
        {
        // Use unchecked to avoid overflow exceptions
        // use prime numbers to reduce collisions
        // 17	19	23	29	31	37	41	43	47	53	59	61	67	71
            int hash = 17; // use prime numbers
            hash = hash * 19 + Status.GetHashCode();
            hash = hash * 23 + Value.GetHashCode();
            return hash;
        }
#endif
    }


    public void Deconstruct(out Guid Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}