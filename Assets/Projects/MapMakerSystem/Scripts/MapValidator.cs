using System;
using Carry.CarrySystem.Spawners.Scripts;
using UnityEngine;

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
        public bool CanSave { get; private set; }
        
        // TODO ブロックが適切に配置されているかどうかをチェックする
        public bool CanPlay()
        {
            return true;
        }

        public bool StartTestPlay(Action onStopped)
        {
            var canPlay = CanPlay();

            if (!canPlay) return false;
            
            _localPlayerSpawner.SpawnPlayer();
            _timerLocal.StartTimer();
            _timerLocal.OnStopped = onStopped;

            return true;
        }

        public void StopTestPlay(bool isClear)
        {
            CanSave = isClear;

            Debug.Log(isClear ? "成功" : "失敗");

            _localPlayerSpawner.DespawnPlayer();
            _timerLocal.StopTimer();
        }
    }
}