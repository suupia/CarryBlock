using Fusion;

# nullable enable
namespace Carry.CarrySystem.CarryScene.Interfaces
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