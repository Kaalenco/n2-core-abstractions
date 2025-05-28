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

    public override int GetHashCode() => HashCode.Combine(Status.GetHashCode(), Value.GetHashCode(StringComparison.Ordinal));

    public override string ToString() => Value;

    public void Deconstruct(out string Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}
