namespace N2.Core.Http;

/// <summary>
/// INterface for an endpoint helper that creates/validates endpoints for an api.
/// </summary>
public interface IApiEndpoint
{
    Uri GetAddress(string endpoint, Dictionary<string, string>? parameters = null);
}
