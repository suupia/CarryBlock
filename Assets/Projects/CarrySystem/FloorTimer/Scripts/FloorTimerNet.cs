using Fusion;

namespace Carry.CarrySystem.FloorTimer.Scripts
{
    public class FloorTimerNet : NetworkBehaviour
    {
        public float CurrentFloorLimitTime { get; } = 60f;

        public float CurrentFloorRemainingTime { get; set; }

        // 元々あったので置いておく。
        // public bool IsExpired { get; set; }
        [Networked] TickTimer TickTimer { get; set; }

        public void StartTimer()
        {
            TickTimer = TickTimer.CreateFromSeconds(Runner, CurrentFloorLimitTime);
        }

        public override void FixedUpdateNetwork()
        {
            CurrentFloorRemainingTime = TickTimer.RemainingTime(Runner).GetValueOrDefault();
            // IsExpired = TickTimer.ExpiredOrNotRunning(Runner);
        }
    }
}