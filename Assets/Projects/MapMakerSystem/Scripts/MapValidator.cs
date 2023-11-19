using Carry.CarrySystem.Spawners.Scripts;

namespace Projects.MapMakerSystem.Scripts
{
    public class MapValidator
    {
        readonly LocalPlayerSpawner _localPlayerSpawner;
        readonly FloorTimerLocal _timerLocal;

        public MapValidator(LocalPlayerSpawner localPlayerSpawner, FloorTimerLocal timerLocal)
        {
            _localPlayerSpawner = localPlayerSpawner;
            _timerLocal = timerLocal;
        }

        // 一旦仮でどんな場合でもSaveできるようにする
        public bool CanSave { get; set; } = true;
        
        // TODO ブロックが適切に配置されているかどうかをチェックする
        public bool CanPlay()
        {
            return true;
        }

        public bool StartTestPlay()
        {
            var canPlay = CanPlay();

            if (!canPlay) return false;
            
            _localPlayerSpawner.SpawnPlayer();
            _timerLocal.StartTimer();

            return true;
        }
    }
}