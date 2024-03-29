namespace N2.Core;

public interface IRequestResult
{
    bool IsSuccessCode { get; }
    string Message { get; }
    int ResultCode { get; }
}
