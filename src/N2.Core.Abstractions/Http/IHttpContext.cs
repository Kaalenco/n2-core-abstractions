using System.Security.Principal;

namespace N2.Core.Http;

public interface IHttpContext
{
    IPrincipal User { get; }
}