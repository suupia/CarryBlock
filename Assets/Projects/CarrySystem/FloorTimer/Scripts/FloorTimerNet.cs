using System;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UnityEngine;
using VContainer;

#nullable enable

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        public float FloorLimitSeconds =>  CalcFloorLimitTime();
        public float FloorRemainingSeconds { get; set; }
        public float FloorRemainingTimeRatio => FloorRemainingSeconds / FloorLimitSeconds;

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
            TickTimer = TickTimer.CreateFromSeconds(Runner, FloorLimitSeconds);
            Debug.Log($"PlayerCount:{_playerCharacterHolder.PlayerCount}"); 
        }

        public override void FixedUpdateNetwork()
        {
            FloorRemainingSeconds = TickTimer.RemainingTime(Runner).GetValueOrDefault();
            IsExpired = TickTimer.ExpiredOrNotRunning(Runner);
        }

        float CalcFloorLimitTime()
        {
            float limitTime = _playerCharacterHolder.PlayerCount switch
            {
                1 => 120,
                2 => 90,
                3 => 70,
                4 => 60,
                _ =>  InvalidPlayerCount(),
            };
            
            return limitTime;
            
            float InvalidPlayerCount()
            {
                Debug.LogError($"PlayerCount : {_playerCharacterHolder.PlayerCount} is invalid.");
                return 60f;
            }
        }
    }
} 