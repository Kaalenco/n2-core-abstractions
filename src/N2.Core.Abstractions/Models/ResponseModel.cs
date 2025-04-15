namespace N2.Core.Models;

public class ResponseModel
{
    public ResponseType Type { get; private set; }
    public string Message { get; private set; }

    public ResponseModel()
    {
        Type = ResponseType.Success;
        Message = string.Empty;
    }

    public ResponseModel(int responseStatus, string message)
    {
        Type = (ResponseType)responseStatus;
        Message = message;
    }

    public ResponseModel(int ResponseStatus) : this(ResponseStatus, string.Empty)
    {
    }

    public bool Success => (int)Type <= 300;

    public virtual bool Equals(ResponseModel? other)
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

    public override string ToString() => $"{Type}, {Message}";

    public static implicit operator int(ResponseModel r) => ToInt32(r);

    public static int ToInt32(ResponseModel r) => r != null ? (int)r.Type : 0;

    public override int GetHashCode() => Type.GetHashCode();

    public int ToInt()
    {
        return ToInt32(this);
    }
}