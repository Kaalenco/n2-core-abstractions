namespace N2.Core.Models;

public class StringResponseModel : ResponseModel, IResult<string>
{
    public string Value { get; }

    public StringResponseModel(string value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public StringResponseModel(string value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public static StringResponseModel NoContent(string Value) => new(Value, 204);

    public static StringResponseModel Fail(string Value, string message) => new(Value, 500, message);

    public static StringResponseModel Accept(string Value) => new(Value, 200);

    public static StringResponseModel NoChange(string Value) => new(Value, 203);

    public virtual bool Equals(IResult<string>? other)
    {
        if (other == null) return false;
        if (other.Value != Value) return false;
        return true;
    }

    public override int GetHashCode() => HashCode.Combine(Type.GetHashCode(), Value.GetHashCode(StringComparison.Ordinal));

    public override string ToString() => Value;

    public void Deconstruct(out string Value, out ResponseType Type)
    {
        Value = this.Value;
        Type = this.Type;
    }
}
