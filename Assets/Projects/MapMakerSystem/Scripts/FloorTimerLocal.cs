using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Projects.MapMakerSystem.Scripts
{
    public class FloorTimerLocal: MonoBehaviour
    {
        const float FLOOR_LIMIT_SECONDS = 120;

        
        public bool IsActive { get; private set; }
        public float FloorRemainingSeconds { get; private set;  }
        public float FloorRemainingTimeRatio => FloorRemainingSeconds / FLOOR_LIMIT_SECONDS;


        public void StartTimer()
        {
            FloorRemainingSeconds = FLOOR_LIMIT_SECONDS;
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