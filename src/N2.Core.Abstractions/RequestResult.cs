namespace N2.Core;

public readonly struct RequestResult : IRequestResult, IEquatable<RequestResult>
{
    private static RequestResult AcceptedResult = new(203, "Accepted");
    private static RequestResult BadRequestResult = new(406, "Bad request");
    private static RequestResult NotFoundResult = new(404, "Not found");
    private static RequestResult OkResult = new(200, "OK");
    private static RequestResult TimeOutResult = new (407, "The request data contains a timestamp that is too old.");

    public RequestResult(int result, string message) : this()
    {
        ResultCode = result;
        Message = message;
    }

    public RequestResult((int, string) init) : this()
    {
        ResultCode = init.Item1;
        Message = init.Item2;
    }

    public readonly bool IsSuccessCode => ResultCode <= 200;

    public string Message { get; } = string.Empty;

    public int ResultCode { get; } = 200;

    public static RequestResult Accepted() => AcceptedResult;

    public static RequestResult Accepted(string message) => new(203, message);

    public static RequestResult BadRequest() => BadRequestResult;

    public static RequestResult NotFound() => NotFoundResult;

    public static RequestResult Ok() => OkResult;
    public static RequestResult Ok(string message) => new(200, message);

    public static bool operator !=(RequestResult left, RequestResult right)
    {
        return !(left == right);
    }

    public static bool operator ==(RequestResult left, RequestResult right)
    {
        return left.Equals(right);
    }

    public static RequestResult TimeOut() => TimeOutResult;
    public readonly bool Equals(RequestResult other)
    {
        return ResultCode == other.ResultCode && Message == other.Message;
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is int x)
        {
            return ResultCode == x;
        }

        if (obj is RequestResult ur)
        {
            return ResultCode == ur.ResultCode && Message == ur.Message;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ResultCode, Message);
    }
}