using System.IO.Pipelines;

namespace N2.Core.Http;

public interface IHttpRequest
{
    PipeReader BodyReader { get; }
    Stream Body { get; }
    string? ContentType { get; }
    long? ContentLength { get; }
    Dictionary<string, string> Headers { get; }
    string Protocol { get; }
    string QueryString { get; }
    string Path { get; }
    string PathBase { get; }
    string Host { get; }
    bool IsHttps { get; }
    string Scheme { get; }
    string Method { get; }
    IHttpContext HttpContext { get; }

    Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default);
}
