namespace Carry.CarrySystem.Map.Scripts
{
    public class EditingMapTransporter
    {
        //編集中のステージIDとマップの順番に相当するインデックスを保持する
        public string StageId { get; private set; } = "test";
        public int Index { get; private set; }
        public void SetEditingMap(string stageId, int index)
        {
            StageId = stageId;
            Index = index;
        }
    }
}