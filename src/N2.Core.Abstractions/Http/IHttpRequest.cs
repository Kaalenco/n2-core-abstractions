using System.Collections.ObjectModel;
using System.IO.Pipelines;

using Microsoft.Extensions.Primitives;

namespace N2.Core.Http;

public interface IHttpRequest
{
    PipeReader BodyReader { get; }
    Stream Body { get; }
    string? ContentType { get; }
    long? ContentLength { get; }
    ReadOnlyDictionary<string, string> Headers { get; }
    string Protocol { get; }
    string QueryString { get; }
    string Path { get; }
    string PathBase { get; }
    string Host { get; }
    bool IsHttps { get; }
    string Scheme { get; }
    string Method { get; }
    IEnumerable<KeyValuePair<string, StringValues>>? Query { get; }

    Task<string> ReadAsStringAsync(CancellationToken cancellationToken = default);
}
