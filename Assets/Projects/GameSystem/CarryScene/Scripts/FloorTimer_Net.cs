using Fusion;
using Carry.CarrySystem.CarryScene.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class FloorTimer_Net : NetworkBehaviour
    {
        readonly float _floorTime = 60f;
        bool _isCounting;
        FloorTimer _floorTimer;
        [Networked] TickTimer tickTimer { get; set; }
    
        [Inject]
        public void Construct(FloorTimer floorTimer)
        {
            _floorTimer = floorTimer;
            Debug.Log($"_waveTimer : {_floorTimer}");
        }
        
        
        public void StartTimer()
        {
            tickTimer = TickTimer.CreateFromSeconds(Runner, _floorTime);
            _isCounting = true;
        }
    
        public override void FixedUpdateNetwork()
        {
            if (_isCounting == false) return;
            // if(Object?.IsValid == false) return;
            // if(tickTimer.ExpiredOrNotRunning(Runner)) tickTimer = TickTimer.CreateFromSeconds(Runner, _floorTime);
            if(tickTimer.ExpiredOrNotRunning(Runner) ) _isCounting = false;
            _floorTimer.tickTimer = tickTimer;
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
    
        public float getRemainingTime(NetworkRunner Runner)
        {
            return tickTimer.RemainingTime(Runner).HasValue ? tickTimer.RemainingTime(Runner).Value : 0f;
        }
    
        public bool isExpired(NetworkRunner Runner)
        {
            return tickTimer.ExpiredOrNotRunning(Runner);
        }
    }
}