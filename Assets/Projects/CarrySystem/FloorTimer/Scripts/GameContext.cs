namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class GameContext
    {
        public enum GameState
        {
            Playing,
            Result // マップを開いている状態や、ユニット選択の状態などが増えるかも
        }

        public GameState CurrentState { get; private set; } = GameState.Playing;
    }
}