using N2.Core.Commands;

namespace N2.Core.Entity;

/// <summary>
/// The health status of the data context.
/// </summary>
public record DataContextHealthStatus
{
    public DataContextHealthStatus(ResponseStatus status, string message)
    {
        StatusCode = (int)status;
        IsHealthy = status.IsSuccess();
        Message = message;
    }
    public bool IsHealthy { get; private set; } = true;
    public string? Message { get; private set; }
    public int? StatusCode { get; private set; }
}