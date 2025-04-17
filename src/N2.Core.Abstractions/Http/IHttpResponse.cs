using System.IO.Pipelines;
using System.Text.Json;

namespace N2.Core.Http;

public interface IHttpResponse
{
    string? ContentType { get; set; }
    long? ContentLength { get; set; }
    PipeWriter BodyWriter { get; }
    Stream Body { get; set; }
    Dictionary<string, string> Headers { get; }
    int StatusCode { get; set; }
    IHttpContext HttpContext { get; }

    void Clear();

    void ClearHeaders();
    void SetHeader(string key, string values);

    Task WriteAsync(string content, CancellationToken cancellationToken = default);
    Task WriteAsync(byte[] content, CancellationToken cancellationToken = default);
    Task WriteAsJsonAsync<T>(T content, JsonSerializerOptions options, CancellationToken cancellationToken = default);
}
