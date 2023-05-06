using Fusion;

public interface ITimerObserver
{
    void Update(NetworkRunner runner, ITimer timer);
}

public interface ITimer
{
    void NotifyObservers(NetworkRunner runner);
    float getRemainingTime(NetworkRunner runner);
    bool isExpired(NetworkRunner runner);
}

public class GameContext : ITimerObserver
{
    public enum GameState
    {
        Playing,
        Result // マップを開いている状態や、ユニット選択の状態などが増えるかも
    }

    public GameState gameState { get; private set; } = GameState.Playing;

    public void Update(NetworkRunner runner, ITimer timer)
    {
        if (timer.isExpired(runner)) gameState = GameState.Result;
    }
}