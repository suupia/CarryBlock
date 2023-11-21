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

        public bool CanSave { get; private set; }

        public void StartTestPlay(Action onStopped)
        {
            _localPlayerSpawner.SpawnPlayer();
            _timerLocal.StartTimer();
            _timerLocal.OnStopped = onStopped;
        }

        public void StopTestPlay(bool isClear)
        {
            CanSave = isClear;

            Debug.Log(isClear ? "成功" : "失敗");

            _localPlayerSpawner.DespawnPlayer();
            _timerLocal.StopTimer();
        }

        public void OnSaved()
        {
            CanSave = false;
        }
    }
}