using Fusion;
using Carry.CarrySystem.CarryScene.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        readonly float _floorTime = 60f;
        bool _isCounting;
        FloorTimer _floorTimer;
        [Networked] TickTimer TickTimer { get; set; }
    
        [Inject]
        public void Construct(FloorTimer floorTimer)
        {
            _floorTimer = floorTimer;
            Debug.Log($"_waveTimer : {_floorTimer}");
        }
        
        
        public void StartTimer()
        {
            TickTimer = TickTimer.CreateFromSeconds(Runner, _floorTime);
            _isCounting = true;
        }
    
        public override void FixedUpdateNetwork()
        {
            if (_isCounting == false) return;
            // if(Object?.IsValid == false) return;
            // if(tickTimer.ExpiredOrNotRunning(Runner)) tickTimer = TickTimer.CreateFromSeconds(Runner, _floorTime);
            if(TickTimer.ExpiredOrNotRunning(Runner) ) _isCounting = false;
            _floorTimer.tickTimer = TickTimer;
            _floorTimer.NotifyObservers(Runner);

        }

    }
    
    public class FloorTimer : ITimer
    {
        readonly GameContext _gameContext;
    
        [Inject]
        public FloorTimer(GameContext gameContext)
        {
            _gameContext = gameContext;
        }
    
        public TickTimer tickTimer { get; set; }
    
        public void NotifyObservers(NetworkRunner runner)
        {
            _gameContext.Update(runner, this);
        }
    
        public float GetRemainingTime(NetworkRunner Runner)
        {
            return tickTimer.RemainingTime(Runner).HasValue ? tickTimer.RemainingTime(Runner).Value : 0f;
        }
    
        public bool IsExpired(NetworkRunner Runner)
        {
            return tickTimer.ExpiredOrNotRunning(Runner);
        }
    }
}