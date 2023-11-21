#nullable enable

using System;
using Carry.CarrySystem.Map.Scripts;

namespace Projects.MapMakerSystem.Scripts
{
    public class MapTestPlayStarter
    {
        readonly StageMapSaver _stageMapSaver;
        readonly MapValidator _mapValidator;
        readonly StageMapSwitcher _stageMapSwitcher;

        public MapTestPlayStarter(
            StageMapSaver stageMapSaver,
            MapValidator mapValidator,
            StageMapSwitcher stageMapSwitcher)
        {
            _stageMapSaver = stageMapSaver;
            _mapValidator = mapValidator;
            _stageMapSwitcher = stageMapSwitcher;
        }

        public bool IsTestPlaying { get; private set; }

        public bool Start(Action onStopped)
        {
            if (IsTestPlaying) return false;
            
            var map = _stageMapSwitcher.GetMap();
            _stageMapSaver.SaveTmpMap(map);

            _mapValidator.StartTestPlay(() =>
            {
                _stageMapSwitcher.InitTmpMap();

                IsTestPlaying = false;
                onStopped.Invoke();
            });

            IsTestPlaying = true;
            return true;
        }
    }
}