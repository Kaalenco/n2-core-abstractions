namespace N2.Core;

public interface IRequestResult
{
    bool IsSuccessCode { get; }
    string Message { get; }
    int ResultCode { get; }
}


/// <summary>
/// Response model for a handler.
/// </summary>
public interface IResponse
{
}

/// <summary>
/// Request model for a handler.
/// </summary>
public interface IRequest
{

}

/// <summary>
/// Handler interface.
/// </summary>
/// <typeparam name="TRequest">A request model.</typeparam>
/// <typeparam name="TResponse">A response model.</typeparam>
public interface IHandle<TRequest, TResponse>
    where TRequest : IRequest
    where TResponse : IResponse
{
    Task<TResponse> Handle(TRequest request, CancellationToken token);
}