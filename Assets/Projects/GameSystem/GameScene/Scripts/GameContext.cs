using Fusion;
using Nuts.Projects.GameSystem.GameScene.Interfaces;

namespace Nuts.Projects.GameSystem.GameScene.Scripts
{
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
}