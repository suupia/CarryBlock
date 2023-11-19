using System;
using UnityEngine;

namespace Projects.MapMakerSystem.Scripts
{
    public class FloorTimerLocal: MonoBehaviour
    {
        bool _isActive = false;
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
            _isActive = true;
        }

        void Update()
        {
            if (!_isActive) return;
            
            FloorRemainingSeconds -= Time.deltaTime;
            if (FloorRemainingSeconds <= 0)
            {
                _isActive = false;
            }
        }
    }
}