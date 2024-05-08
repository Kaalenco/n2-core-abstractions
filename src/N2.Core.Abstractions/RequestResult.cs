namespace N2.Core;

public readonly struct RequestResult : IRequestResult, IEquatable<RequestResult>
{
    public const int AcceptedCode = 203;
    public const int BadRequestCode = 406;
    public const int NotFoundCode = 404;
    public const int OkCode = 200;
    public const int UnauthorizedCode = 403;
    public const int UnexpectedCode = 500;
    public const int TimeOutCode = 407;

    private static RequestResult AcceptedResult = new(AcceptedCode, "Accepted");
    private static RequestResult UnauthorizedResult = new(UnauthorizedCode, "Unauthorized");
    private static RequestResult BadRequestResult = new(BadRequestCode, "Bad request");
    private static RequestResult NotFoundResult = new(NotFoundCode, "Not found");
    private static RequestResult UnexpectedResult = new(UnexpectedCode, "Unexpected exception");
    private static RequestResult OkResult = new(OkCode, "OK");
    private static RequestResult TimeOutResult = new (TimeOutCode, "The request data contains a timestamp that is too old.");

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

    public readonly bool IsSuccessCode => ResultCode <= OkCode;

    public string Message { get; } = string.Empty;

    public int ResultCode { get; } = OkCode;

    public static RequestResult Accepted() => AcceptedResult;
    public static RequestResult Accepted(string message) => new(AcceptedCode, message);

    public static RequestResult BadRequest() => BadRequestResult;

    public static RequestResult NotFound() => NotFoundResult;
    public static RequestResult NotFound(string message) => new(NotFoundCode, message);

    public static RequestResult Unauthorized() => UnauthorizedResult;
    public static RequestResult Unauthorized(string message) => new(UnauthorizedCode, message);

    public static RequestResult Ok() => OkResult;
    public static RequestResult Ok(string message) => new(OkCode, message);

    public static RequestResult Unexpected() => UnexpectedResult;
    public static RequestResult Unexpected(string message) => new(UnexpectedCode, message);

    public static RequestResult TimeOut() => TimeOutResult;

    public static bool operator !=(RequestResult left, RequestResult right)
    {
        return !(left == right);
    }

    public static bool operator ==(RequestResult left, RequestResult right)
    {
        return left.Equals(right);
    }

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