using Carry.CarrySystem.Block.Interfaces;
# nullable enable

namespace Carry.CarrySystem.Gimmick.Interfaces
{
    public interface IGimmickBlock : IBlock
    {
        public void StartGimmick();
        public void EndGimmick();
    }
}