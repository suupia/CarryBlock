using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        public float FloorLimitTime =>  CalcFloorLimitTime();
        public float FloorRemainingTime { get; set; }
        public float FloorRemainingTimeRatio => FloorRemainingTime / FloorLimitTime;

        public bool IsExpired { get; set; } 
        [Networked] TickTimer TickTimer { get; set; }

        PlayerCharacterHolder _playerCharacterHolder = null!;

        [Inject]
        public void Construct(PlayerCharacterHolder playerCharacterHolder)
        {
            _playerCharacterHolder = playerCharacterHolder;
        }

        public void StartTimer()
        {
            TickTimer = TickTimer.CreateFromSeconds(Runner, FloorLimitTime);
            Debug.Log($"PlayerCount:{_playerCharacterHolder.PlayerCount}"); 
        }

        public override void FixedUpdateNetwork()
        {
            FloorRemainingTime = TickTimer.RemainingTime(Runner).GetValueOrDefault();
            IsExpired = TickTimer.ExpiredOrNotRunning(Runner);
        }

        float CalcFloorLimitTime()
        {
            var limitTime = _playerCharacterHolder.PlayerCount switch
            {
                1 => 120,
                2 => 90,
                3 => 70,
                4 => 60,
                _ => 600  // 適当
            };

            return limitTime;
        }
    }
} 