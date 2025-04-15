namespace N2.Core.Models;

public class GuidResponseModel : ResponseModel, IResult<Guid>
{
    public Guid Value { get; }

    public GuidResponseModel(Guid value, int responseStatus, string message) : base(responseStatus, message)
    {
        Value = value;
    }

    public GuidResponseModel(Guid value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public static GuidResponseModel Fail(Guid Value, string message) => new(Value, 500, message);

    public static GuidResponseModel Accept(Guid Value) => new(Value, 200);

    public static GuidResponseModel Created(Guid Value) => new(Value, 201);

    public static GuidResponseModel NoChanges(Guid Value, string message) => new(Value, 204, message);

    public virtual bool Equals(IResult<Guid>? other)
    {
        if (other == null) return false;
        if (other.Value != Value) return false;
        return true;
    }

    public override int GetHashCode() => HashCode.Combine(Type.GetHashCode(), Value.GetHashCode());

    public void Deconstruct(out Guid Value, out ResponseType Type)
    {
        Value = this.Value;
        Type = this.Type;
    }
}