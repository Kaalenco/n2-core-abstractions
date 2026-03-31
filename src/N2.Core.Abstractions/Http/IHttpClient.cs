using System.Net.Http.Headers;
using System.Text.Json;

namespace N2.Core.Http;

/// <summary>
/// Abstraction for http client.
/// </summary>
public interface IHttpClient
{
    /// <summary>
    /// The tome out for any response.
    /// </summary>
    TimeSpan Timeout { get; set; }

    /// <summary>
    /// The base address for the client.
    /// </summary>
    Uri? BaseAddress { get; set; }

    /// <summary>
    /// Serializer options for request and response.
    /// </summary>
    JsonSerializerOptions Options { get; set; }

    /// <summary>
    /// Default headers for this http client.
    /// </summary>
    HttpRequestHeaders DefaultRequestHeaders { get; }

    /// <summary>
    /// Performs DELETE operation to a given URI.
    /// </summary>
    /// <param name="path">The DELETE endpoint, relative to the base address.</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>
    /// A http message that should reflect the result.
    /// Default response for a delete would be a '204 No-Content'
    /// indicating a successful deletion of the resource.
    /// </returns>
    Task<IHttpResult> DeleteAsync(string path, CancellationToken token);

    /// <summary>
    /// Performs a GET operation relative from the base address.
    /// </summary>
    /// <typeparam name="TResponse">Expected type of content</typeparam>
    /// <param name="path">The GET endpoint, relative to the base address.</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns></returns>
    Task<IHttpResult<TResponse>> GetRelativeAsync<TResponse>(string path, CancellationToken token);

    /// <summary>
    /// Read a json document from a resource.
    /// </summary>
    /// <param name="resourcePath">The fully unified resource indicator.</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns></returns>
    Task<JsonDocument> ReadJsonDocumentAsync(Uri resourcePath, CancellationToken token);

    /// <summary>
    /// Performs a GET operation using a full resource path.
    /// </summary>
    /// <typeparam name="TResource">Expected type of resource.</typeparam>
    /// <param name="resourcePath">The fully unified resource indicator.</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns></returns>
    Task<IHttpResult<TResource>> GetFromUriAsync<TResource>(Uri resourcePath, CancellationToken token);

    /// <summary>
    /// Performs a POST operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a POST indicates the creating of a resource.
    /// </summary>
    /// <typeparam name="TResponse">Status of expected response</typeparam>
    /// <typeparam name="TRequest">Status of the message body</typeparam>
    /// <param name="path">The POST endpoint, relative to the base address.</param>
    /// <param name="value">The body of the message</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResponse>> PostRelativeAsync<TRequest, TResponse>(string path, TRequest value, CancellationToken token);

    /// <summary>
    /// Performs a POST operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a POST indicates the creating of a resource.
    /// </summary>
    /// <typeparam name="TResource">Status of the resource</typeparam>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="value">The body of the message</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResource>> PostResourceAsync<TResource>(Uri resourcePath, TResource value, CancellationToken token);

    /// <summary>
    /// Performs a POST operation to a given resource path and returns the http response message.
    /// This is a primitive operation that does not attempt to deserialize the response content.
    /// It is useful when the caller needs to handle the response message directly, for example, 
    /// to check status codes or read headers without deserializing the body.
    /// </summary>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="content">The HTTP content of the message</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>An Http result (<see cref="HttpResponseMessage" />)</returns>
    Task<HttpResponseMessage> PostAsync<TResource>(Uri resourcePath, HttpContent content, CancellationToken token);

    /// <summary>
    /// Performs a GET operation to a given resource path and returns the http response message.
    /// This is a primitive operation that does not attempt to deserialize the response content.
    /// It is useful when the caller needs to handle the response message directly, for example, 
    /// to check status codes or read headers without deserializing the body.
    /// </summary>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    Task<HttpResponseMessage> GetAsync<TResource>(Uri resourcePath, CancellationToken token);

    /// <summary>
    /// Performs a PUT operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a PUT indicates updating a resource.
    /// </summary>
    /// <typeparam name="TResponse">Status of expected response</typeparam>
    /// <typeparam name="TRequest">Status of the message body</typeparam>
    /// <param name="path">The POST endpoint, relative to the base address.</param>
    /// <param name="value">The body of the message</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResponse>> PutRelativeAsync<TRequest, TResponse>(string path, TRequest value, CancellationToken token);

    /// <summary>
    /// Performs a PUT operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a PUT indicates updating a resource.
    /// </summary>
    /// <typeparam name="TResource">Status of the resource</typeparam>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="value">The body of the message</param>
    /// <param name="token">A cancellation token to cancel the request.</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResource>> PutResourceAsync<TResource>(Uri resourcePath, TResource value, CancellationToken token);
}
