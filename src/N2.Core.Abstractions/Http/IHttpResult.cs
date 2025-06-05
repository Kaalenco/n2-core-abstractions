using System.Net;

namespace N2.Core.Http;

/*
 * Sample headers:
200 OK
Access-Control-Allow-Origin: *
Connection: Keep-Alive
Content-Encoding: gzip
Content-Status: text/html; charset=utf-8
Date: Mon, 18 Jul 2016 16:06:00 GMT
Etag: "c561c68d0ba92bbeb8b0f612a9199f722e3a621a"
Keep-Alive: timeout=5, max=997
Last-Modified: Mon, 18 Jul 2016 02:36:04 GMT
Server: Apache
Set-Cookie: my-key=my value; expires=Mon, 17-Jul-2017 16:06:00 GMT; Max-Age=31449600; Path=/; secure
Transfer-Encoding: chunked
Vary: Cookie, Accept-Encoding
X-Backend-Server: developer2.webapp.scl3.mozilla.com
X-Cache-Info: not cacheable; meta data too large
X-kuma-revision: 1085259
x-frame-options: DENY
 */

/// <summary>
/// Abstraction for http results.
/// </summary>
public interface IHttpResult
{
    /// <summary>
    /// The status code for the response, should be returned as
    /// a message header, followed by the description. e.g. '200 OK'
    /// </summary>
    HttpStatusCode StatusCode { get; }

    /// <summary>
    /// An optional response message that gives more information
    /// on the response.
    /// </summary>
    string? Message { get; }

    /// <summary>
    /// A string containing a tag that can be used for audit tracking or
    /// for a client to relate responses to a call.
    /// </summary>
    [Obsolete("Use AuditTag instead.This will be removed in a future version.")]
    string? Etag { get; }

    /// <summary>
    /// A string containing a tag that can be used for audit tracking or
    /// for a client to relate responses to a call.
    /// </summary>
    string? AuditTag { get; }

    /// <summary>
    /// A reference that could be used to reference a created or modified object
    /// or any other Uniform Resource Identifier.
    /// </summary>
    Uri? ReferenceUri { get; }

    /// <summary>
    /// Check if the response code is a code that indicates succes.
    /// </summary>
    /// <returns>true if the StatusCode indicates success (2nn codes)</returns>
    bool IsSuccess();

    /// <summary>
    /// This property is set to true if the transmission failed due to
    /// a serialization error on transmission.
    /// </summary>
    bool SerializationError { get; }
}

public interface IHttpResult<T> : IHttpResult
{
    /// <summary>
    /// The content should be serialized to an http content if its not a type that
    /// derives from <see cref="HttpContent"/>. Most types should be serialized
    /// to either json or binary data.
    /// </summary>
    T Result { get; }
}