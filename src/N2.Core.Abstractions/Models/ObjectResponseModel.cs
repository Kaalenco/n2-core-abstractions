
namespace N2.Core.Models;

public class ObjectResponseModel<T> : ResponseModel, IResult<T> where T : new()
{
    public T Value { get; }

    public ObjectResponseModel() : base(500, string.Empty)
    {
        Value = new();
    }

    public ObjectResponseModel(T value, int responseStatus, string Message) : base(responseStatus, Message)
    {
        Value = value;
    }

    public ObjectResponseModel(T value, int responseStatus) : base(responseStatus)
    {
        Value = value;
    }

    public bool Equals(IResult<T>? other)
    {
        if (other == null)
        {
            return false;
        }

        if (other.Type != Type)
        {
            return false;
        }

        return true;
    }

    public ObjectResponseModel<T> Fail(string message) => new(new(), 500, message);

    public ObjectResponseModel<T> Accept(T Value) => new(Value, 200);

    public override int GetHashCode() => HashCode.Combine(Type.GetHashCode(), Value?.GetHashCode());

    public void Deconstruct(out T Value, out ResponseType Type)
    {
        Value = this.Value;
        Type = this.Type;
    }
}
