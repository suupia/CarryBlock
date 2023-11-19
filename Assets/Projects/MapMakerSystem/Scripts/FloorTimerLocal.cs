using System;
using UnityEngine;

namespace Projects.MapMakerSystem.Scripts
{
    public class FloorTimerLocal: MonoBehaviour
    {
        public bool IsActive { get; private set; }
        public float FloorLimitSeconds { get; }
        public float FloorRemainingSeconds { get; private set;  }
        public float FloorRemainingTimeRatio => FloorRemainingSeconds / FloorLimitSeconds;

        public FloorTimerLocal(float limitSecond)
        {
            FloorLimitSeconds = limitSecond;
        }
        
        public void StartTimer()
        {
            FloorRemainingSeconds = FloorLimitSeconds;
            IsActive = true;
        }

        void Update()
        {
            if (!IsActive) return;
            
            FloorRemainingSeconds -= Time.deltaTime;
            if (FloorRemainingSeconds <= 0)
            {
                IsActive = false;
            }
        }
    }
}