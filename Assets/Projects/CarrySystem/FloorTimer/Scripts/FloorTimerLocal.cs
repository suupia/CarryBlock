#nullable enable
using System;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using Projects.CarrySystem.FloorTimer.Interfaces;
using UnityEngine;
using VContainer;

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerLocal : MonoBehaviour, IFloorTimer
    {
        public float FloorLimitSeconds =>  CalcFloorLimitTime();
        public float FloorRemainingSeconds { get; set; }
        public float FloorRemainingTimeRatio => FloorRemainingSeconds / FloorLimitSeconds;
        public float FloorRemainingSecondsSam { get; set; }

        public bool IsExpired { get; set; } 
        public bool IsCleared { get; set; } 
        [Networked] TickTimer TickTimer { get; set; }

        PlayerCharacterTransporter _playerCharacterTransporter = null!;

        [Inject]
        public void Construct(PlayerCharacterTransporter playerCharacterTransporter)
        {
            _playerCharacterTransporter = playerCharacterTransporter;
        }

        public void StartTimer()
        {
            // TickTimer = TickTimer.CreateFromSeconds(Runner, FloorLimitSeconds);
            FloorRemainingSeconds = FloorLimitSeconds;
            Debug.Log($"PlayerCount:{_playerCharacterTransporter.PlayerCount}"); 
        }

        public void SumRemainingTime()
        {
            FloorRemainingSecondsSam += Mathf.Floor(FloorRemainingSeconds);
        }

        void FixedUpdate()
        {
            // FloorRemainingSeconds = TickTimer.RemainingTime(Runner).GetValueOrDefault();
            // IsExpired = TickTimer.Expired(Runner);
            FloorRemainingSeconds -= Time.fixedDeltaTime;
            if (FloorRemainingSeconds <= 0)
            {
                IsExpired = true;
            }
        }

        float CalcFloorLimitTime()
        {
            
            float limitTime = _playerCharacterTransporter.PlayerCount switch
            {
                0 => 1000, // シーン読み込み時は0なので、ここでエラーを投げたくないので適当な値を入れておく
                1 => 120,
                2 => 100,
                3 => 90,
                4 => 80,
                _ =>  InvalidPlayerCount(),
            };
            
            return limitTime;
            
            float InvalidPlayerCount()
            {
                Debug.LogError($"PlayerCount : {_playerCharacterTransporter.PlayerCount} is invalid.");
                return 60f;
            }
        }
    }
}