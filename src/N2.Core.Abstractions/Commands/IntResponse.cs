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

    public override int GetHashCode() => HashCode.Combine(Status.GetHashCode(), Value.GetHashCode());

    public int Value { get; }
}