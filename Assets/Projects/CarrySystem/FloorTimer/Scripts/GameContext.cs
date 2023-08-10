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

        //ランダム性がないと仮定し、フロア数から制限時間を返す関数
        //SliderのMaxValueの設定時に使用する
        public float CurrentFloorLimitTime => 60;

        public int CurrentFloor { get; set; }

        public float CurrentFloorTime { get; set; }
    }
}