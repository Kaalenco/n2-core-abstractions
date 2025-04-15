namespace N2.Core;

public interface IBackgroundWorker
{
    void StartJob();
    void StopJob();
    void Restart();
    void PauseJob();
    void ContinueJob();
    bool IsRunning();
    bool IsWaiting();
    IWorkerStatus CurrentStatus();
}