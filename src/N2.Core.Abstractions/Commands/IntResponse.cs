namespace N2.Core.Commands;

public class IntResponse : CommandResponse, ICommandResponse<int>
{
    public IntResponse(int value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public IntResponse(int value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public void Deconstruct(out int Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }

    public virtual bool Equals(ICommandResponse<int>? other)
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

    public int Value { get; }
}