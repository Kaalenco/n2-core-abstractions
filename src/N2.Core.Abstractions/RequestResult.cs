using System.Text.Json.Serialization;

using N2.Core.Commands;

namespace N2.Core;

public static class RequestResultExtensions
{
    public static RequestResult WithHandle(RequestResult requestResult, Guid handle)
    {
        string? message = requestResult.Message;
        if (string.IsNullOrEmpty(message))
        {
            message = requestResult.ToString();
            if (string.IsNullOrEmpty(message))
            {
                message = "No message provided.";
            }
        }

        return new RequestResult(
            requestResult.Status,
            requestResult.MessageOrDefault(),
            handle);
    }

    public static string MessageOrDefault(this RequestResult requestResult, string defaultMessage = "No message provided.")
    {
        if (!string.IsNullOrEmpty(requestResult.Message))
        {
            return requestResult.Message!;
        }

        if (!string.IsNullOrEmpty(defaultMessage))
        {
            return defaultMessage;
        }

        string? fallbackMessage = requestResult.ToString();

        return !string.IsNullOrEmpty(fallbackMessage) ? fallbackMessage : "No message provided.";
    }
}

public readonly struct RequestResult : ICommandResponse, IEquatable<RequestResult>
{
    public const int AcceptedCode = 203;
    public const int BadRequestCode = 406;
    public const int NotFoundCode = 404;
    public const int OkCode = 200;
    public const int UnauthorizedCode = 403;
    public const int UnexpectedCode = 500;
    public const int TimeOutCode = 407;

    private static readonly RequestResult AcceptedResult = new(AcceptedCode, "Accepted");
    private static readonly RequestResult UnauthorizedResult = new(UnauthorizedCode, "Unauthorized");
    private static readonly RequestResult BadRequestResult = new(BadRequestCode, "Bad request");
    private static readonly RequestResult NotFoundResult = new(NotFoundCode, "Not found");
    private static readonly RequestResult UnexpectedResult = new(UnexpectedCode, "Unexpected exception");
    private static readonly RequestResult OkResult = new(OkCode, "OK");
    private static readonly RequestResult TimeOutResult = new(TimeOutCode, "The request data contains a timestamp that is too old.");

    public RequestResult(int result, string message) : this()
    {
        Status = (ResponseStatus)result;
        Message = message;
    }

    public RequestResult(ResponseStatus result, string message) : this()
    {
        Status = result;
        Message = message;
    }

    public RequestResult(ResponseStatus result, string message, Guid handle) : this()
    {
        Status = result;
        Message = message;
        Handle = handle;
    }

    public RequestResult((int, string) init) : this()
    {
        Status = (ResponseStatus)init.Item1;
        Message = init.Item2;
    }

    public readonly bool IsSuccessCode => Status < ResponseStatus.NotAccepted;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public long? ExecutionTime { get; } = null;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TrackingId? Handle { get; } = null;

    public ResponseStatus Status { get; } = (ResponseStatus)OkCode;
    public int Code => (int)Status;

    public static RequestResult Accepted() => AcceptedResult;

    public static RequestResult Accepted(string message) => new(AcceptedCode, message);

    public static RequestResult BadRequest() => BadRequestResult;

    public static RequestResult BadRequest(string message) => new(BadRequestCode, message);

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
        return Status == other.Status && Message == other.Message;
    }

    public override readonly bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj is int x)
        {
            return Code == x;
        }

        if (obj is RequestResult ur)
        {
            return Status == ur.Status && Message == ur.Message;
        }

        return false;
    }

    public override int GetHashCode()
    {
#if NETSTANDARD2_1_OR_GREATER
        return HashCode.Combine(Status, Message);
#else

#pragma warning disable CA1307 // For GetHashCode, there is no stringcomparison for netstandard 2.0

        unchecked
        {
            int hash = 17;
            hash = (hash * 23) + Status.GetHashCode();
            hash = (hash * 23) + (Message?.GetHashCode() ?? 0);
            return hash;
        }

#pragma warning restore CA1307

#endif
    }

    public ICommandResponse CreateNew(ResponseStatus status, string? message = null, Guid? handle = null, long? executionTime = null)
    {
        string outputMessage = string.Empty;
        if (string.IsNullOrEmpty(message))
        {
            message = this.ToString();
            if (string.IsNullOrEmpty(message))
            {
                outputMessage = "No message provided.";
            }
        }
        else
        {
            outputMessage = message!;
        }

        if (handle is Guid handleGuid)
        {
            return new RequestResult(status, outputMessage, handleGuid);
        }
        else
        {
            return new RequestResult(status, outputMessage);
        }
    }

    public object Clone()
    {
        return new RequestResult(Status, Message ?? "No message provided.", Handle?.Value ?? Guid.Empty);
    }
}