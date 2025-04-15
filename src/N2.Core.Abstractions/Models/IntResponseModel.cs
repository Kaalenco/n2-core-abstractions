namespace N2.Core.Models;

public class IntResponseModel : ResponseModel, IResult<int>
{
    public IntResponseModel(int value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public IntResponseModel(int value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public void Deconstruct(out int Value, out ResponseType Type)
    {
        Value = this.Value;
        Type = this.Type;
    }

    public virtual bool Equals(IResult<int>? other)
    {
        if (other == null) return false;
        if (other.Value != Value) return false;
        return true;
    }

    public override int GetHashCode() => HashCode.Combine(Type.GetHashCode(), Value.GetHashCode());

    public int Value { get; }
}