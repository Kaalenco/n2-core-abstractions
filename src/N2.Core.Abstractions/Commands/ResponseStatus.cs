namespace N2.Core.Commands;

/// <summary>
/// <para>
/// The response status, an abstract of the HTTP response status list:
/// </para>
/// <para>
/// https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
/// </para>
/// <para>
/// The status can be used to implement stateful responses (ReST : Representational State Transfer).
/// https://en.wikipedia.org/wiki/Representational_state_transfer
/// </para>
/// </summary>
public enum ResponseStatus
{
    None = 0,
    /// <summary>
    /// Indicating a successful process result.
    /// </summary>
    Success = 200,

    /// <summary>
    /// A new element is created, the response header should
    /// contain an HTTP reference to the new content.
    /// </summary>
    Created = 201,

    /// <summary>
    /// The request is accepted, but the process is not completed.
    /// The response header should contain an HTTP reference to an process status retrieval endpoint.
    /// </summary>
    Accepted = 202,

    /// <summary>
    /// Officially, this status indicates Non-Authoritative Information, but it may be used to indicate
    /// that a request is accepted and queued for processing.
    /// </summary>
    Information = 203,

    /// <summary>
    /// The request was successful, but did not return a response.
    /// This can be the case after delete commands or other commands
    /// that do not yield any results.
    /// </summary>
    NoContent = 204,

    /// <summary>
    /// This status indicates that previous requested changes, are reversed.
    /// </summary>
    ResetContent = 205,

    /// <summary>
    /// The response indicates that no changes were made.
    /// </summary>
    NotAccepted = 300,
    NotModified = 304,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    MethodNotAllowed = 405,
    NotAcceptable = 406,
    TimeOut = 408,
    Conflict = 409,
    PreconditionFailed = 412,
    Locked = 423,
    ToomanyRequests = 429,
    ServerError = 500,
    NotImplemented = 501,
    ServiceUnavailable = 503
}
