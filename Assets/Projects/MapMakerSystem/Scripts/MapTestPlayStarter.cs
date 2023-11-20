using System;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;
#nullable enable

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
            var map = _stageMapSwitcher.GetMap();
            _stageMapSaver.SaveTmpMap(map);

            var canPlay = _mapValidator.StartTestPlay(() =>
            {
                var preStage = _stageMapSwitcher.Stage;
                _stageMapSwitcher.Stage = StageFileUtility.Load(StageFileUtility.TMPStageID)!;
                _stageMapSwitcher.InitSwitchMap();
                _stageMapSwitcher.Stage = preStage;

                IsTestPlaying = false;
                onStopped.Invoke();
            });

            if (canPlay)
            {
                IsTestPlaying = true;
                return true;
            }

            Debug.LogWarning("ブロックが適切に置かれていないためプレイできません");
            return false;
        }
    }
}