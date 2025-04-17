namespace N2.Core.Http;

public interface IHttpContextAccessor
{
    IHttpContext HttpContext { get; }
}