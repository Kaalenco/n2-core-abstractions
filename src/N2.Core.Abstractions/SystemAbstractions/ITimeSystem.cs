namespace N2.Core.SystemAbstractions;

/// <summary>
/// Wrapper for System.DateTime or any other time provider.
/// </summary>
public interface ITimeSystem
{
    /// <summary>
    /// Gets the current date and time in utc.
    /// </summary>
    DateTime UtcNow { get; }

    /// <summary>
    /// Gets the current date and time in local time.
    /// </summary>
    DateTimeOffset LocalNow { get; }

    /// <summary>
    /// Gets the current date, without time information.
    /// The method returns the local date, not the date in UTC.
    /// </summary>
    DateTime LocalToday { get; }
}