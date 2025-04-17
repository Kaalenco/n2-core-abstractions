using System.Security.Principal;

namespace N2.Core.Http;

/// <summary>
/// Abstraction for the http context, with only the properties
/// that are actually used (for now).
/// </summary>
public interface IHttpContext
{
    IPrincipal User { get; }
}