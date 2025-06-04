namespace N2.Core;

public static class CommonExtensions
{
    /// <summary>
    /// Returns a Unix epoch time (jan 1st 1970 UTC).
    /// </summary>
    /// <param name="dateTime">
    /// Placeholder
    /// </param>
    /// <returns>
    /// The UNIX Epoch
    /// </returns>
    public static DateTime UnixEpoch(this DateTime dateTime) => new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Returns a Unix epoch time (jan 1st 1970 UTC).
    /// </summary>
    /// <returns>
    /// The UNIX Epoch
    /// </returns>
    public static DateTime UnixEpoch() => new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}