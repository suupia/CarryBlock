#nullable enable
using System;
using UnityEngine;

namespace Projects.MapMakerSystem.Scripts
{
    public class FloorTimerLocal: MonoBehaviour
    {
        //現在カートが動かないので、カートの移動時間を減らした秒数
        readonly float FLOOR_LIMIT_SECONDS = 100;

        public bool IsActive { get; private set; }
        public float FloorRemainingSeconds { get; private set;  }
        public float FloorRemainingTimeRatio => FloorRemainingSeconds / FLOOR_LIMIT_SECONDS;

        public Action OnStopped = () => { };

        public void StartTimer()
        {
            FloorRemainingSeconds = FLOOR_LIMIT_SECONDS;
            IsActive = true;
        }

        public void CancelTimer()
        {
            IsActive = false;
        }

        void Update()
        {
            if (!IsActive) return;
            
            FloorRemainingSeconds -= Time.deltaTime;
            if (FloorRemainingSeconds <= 0)
            {
                IsActive = false;
                OnStopped();
            }
        }
    }
}