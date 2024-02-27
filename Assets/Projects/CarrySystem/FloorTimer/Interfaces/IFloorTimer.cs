#nullable enable
namespace Projects.CarrySystem.FloorTimer.Interfaces
{
    public interface IFloorTimer
    {
        public float FloorLimitSeconds { get; }
        public float FloorRemainingSeconds { get; set; }
        public float FloorRemainingTimeRatio { get; }
        public float FloorRemainingSecondsSam { get; set; }

        public bool IsExpired { get; set; } 
        public bool IsCleared { get; set; } 
        public void StartTimer();

        public void SumRemainingTime();
    }
}