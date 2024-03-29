namespace N2.Core;

public interface ILogService
{
    void LogDebug<T>(string message);

    void LogInformation<T>(string message);

    void LogWarning<T>(string message);

    void LogError<T>(string message);

    void LogCritical<T>(string message);

    void LogEvent<T>(string message, string category);

    //void LogEvent(string message, string logCategory, string logType, string appName, string? version = null)
}