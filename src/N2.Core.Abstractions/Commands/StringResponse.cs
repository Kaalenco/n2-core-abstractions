namespace N2.Core.Commands;

public class StringResponse : CommandResponse, ICommandResponse<string>
{
    public string Value { get; }

    public StringResponse(string value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public StringResponse(string value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public static StringResponse NoContent(string Value) => new(Value, 204);

    public static StringResponse Fail(string Value, string message) => new(Value, 500, message);

    public static StringResponse Accept(string Value) => new(Value, 200);

    public static StringResponse NoChange(string Value) => new(Value, 203);

    public virtual bool Equals(ICommandResponse<string>? other)
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
        return HashCode.Combine(Status.GetHashCode(), Value.GetHashCode(StringComparison.Ordinal));
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

    public override string ToString() => Value;

    public void Deconstruct(out string Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}
