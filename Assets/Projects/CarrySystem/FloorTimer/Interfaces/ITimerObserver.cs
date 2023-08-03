using Fusion;

namespace Projects.CarrySystem.FloorTimer.Interfaces
{
    public interface ITimerObserver
    {
        void Update(NetworkRunner runner, ITimer timer);
    }

    public interface ITimer
    {
        void NotifyObservers(NetworkRunner runner);
        float GetRemainingTime(NetworkRunner runner);
        bool IsExpired(NetworkRunner runner);
    }
}