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

    public override int GetHashCode() => HashCode.Combine(Status.GetHashCode(), Value.GetHashCode());

    public void Deconstruct(out Guid Value, out ResponseStatus Status)
    {
        Value = this.Value;
        Status = this.Status;
    }
}