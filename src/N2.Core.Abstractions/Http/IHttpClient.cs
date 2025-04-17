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
    /// <returns>
    /// A http message that should reflect the result.
    /// Default response for a delete would be a '204 No-Content'
    /// indicating a successful deletion of the resource.
    /// </returns>
    Task<IHttpResult> DeleteAsync(string path);

    /// <summary>
    /// Performs a GET operation relative from the base address.
    /// </summary>
    /// <typeparam name="TResponse">Expected type of content</typeparam>
    /// <param name="path">The GET endpoint, relative to the base address.</param>
    /// <returns></returns>
    Task<IHttpResult<TResponse>> GetRelativeAsync<TResponse>(string path);

    /// <summary>
    /// Performs a GET operation using a full resource path.
    /// </summary>
    /// <typeparam name="TResource">Expected type of resource.</typeparam>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <returns></returns>
    Task<IHttpResult<TResource>> GetFromUriAsync<TResource>(Uri resourcePath);

    /// <summary>
    /// Performs a POST operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a POST indicates the creating of a resource.
    /// </summary>
    /// <typeparam name="TResponse">Type of expected response</typeparam>
    /// <typeparam name="TRequest">Type of the message body</typeparam>
    /// <param name="path">The POST endpoint, relative to the base address.</param>
    /// <param name="value">The body of the message</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResponse>> PostRelativeAsync<TRequest, TResponse>(string path, TRequest value);

    /// <summary>
    /// Performs a POST operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a POST indicates the creating of a resource.
    /// </summary>
    /// <typeparam name="TResource">Type of the resource</typeparam>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="value">The body of the message</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResource>> PostResourceAsync<TResource>(Uri resourcePath, TResource value);

    /// <summary>
    /// Performs a PUT operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a PUT indicates updating a resource.
    /// </summary>
    /// <typeparam name="TResponse">Type of expected response</typeparam>
    /// <typeparam name="TRequest">Type of the message body</typeparam>
    /// <param name="path">The POST endpoint, relative to the base address.</param>
    /// <param name="value">The body of the message</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResponse>> PutRelativeAsync<TRequest, TResponse>(string path, TRequest value);

    /// <summary>
    /// Performs a PUT operation to a given relative path and deserializes
    /// its result or null when it fails. If the endpoint is a REST endpoint,
    /// a PUT indicates updating a resource.
    /// </summary>
    /// <typeparam name="TResource">Type of the resource</typeparam>
    /// <param name="resourcePath">The full unified resource indicator.</param>
    /// <param name="value">The body of the message</param>
    /// <returns>An Http result (<see cref="IHttpResult" />)</returns>
    Task<IHttpResult<TResource>> PutResourceAsync<TResource>(Uri resourcePath, TResource value);
}
