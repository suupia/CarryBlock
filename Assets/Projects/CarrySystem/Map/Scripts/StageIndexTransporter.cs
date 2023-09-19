namespace Carry.CarrySystem.Map.Scripts
{
    public class StageIndexTransporter
    {
        public int StageIndex { get; private set; } = -1;
        public void SetStageIndex(int index)
        {
            StageIndex = index;
        }
    }
}