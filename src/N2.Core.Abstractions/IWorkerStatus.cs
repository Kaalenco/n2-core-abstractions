namespace N2.Core;

public interface IWorkerStatus
{
    bool IsRunning { get; }
    bool IsWaiting { get; }
    bool ErrorOccured { get; }
    string ErrorMessage { get; }
    string? ErrorStackTrace { get; }
    string? ErrorSource { get; }
    int ErrorCode { get; }
    string CurrentAction { get; }
    DateTime StartTimeUtc { get; }
    DateTime RunStartedUtc { get; }
    DateTime NextRunTimeUtc { get; }
    TimeSpan ElapsedTime { get; }
}