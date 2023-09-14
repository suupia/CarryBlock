using Fusion;
#nullable enable

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        public float FloorLimitTime { get; } = 3f;

        public float FloorRemainingTime { get; set; }

        public bool IsExpired { get; set; } 
        [Networked] TickTimer TickTimer { get; set; }

        public void StartTimer()
        {
            TickTimer = TickTimer.CreateFromSeconds(Runner, FloorLimitTime);
        }

        public override void FixedUpdateNetwork()
        {
            FloorRemainingTime = TickTimer.RemainingTime(Runner).GetValueOrDefault();
            IsExpired = TickTimer.ExpiredOrNotRunning(Runner);
        }
    }
}