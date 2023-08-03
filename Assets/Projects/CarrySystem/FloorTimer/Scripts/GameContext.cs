using Fusion;
using Projects.CarrySystem.FloorTimer.Interfaces;

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class GameContext : ITimerObserver
    {
        public enum GameState
        {
            Playing,
            Result // マップを開いている状態や、ユニット選択の状態などが増えるかも
        }

        public GameState CurrentState { get; private set; } = GameState.Playing;

        public void Update(NetworkRunner runner, ITimer timer)
        {
            if (timer.IsExpired(runner)) CurrentState = GameState.Result;
        }
    }
}