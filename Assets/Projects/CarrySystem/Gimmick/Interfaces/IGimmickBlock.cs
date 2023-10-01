using Carry.CarrySystem.Block.Interfaces;
using Fusion;

# nullable enable

namespace Carry.CarrySystem.Gimmick.Interfaces
{
    public interface IGimmickBlock : IBlock
    {
        public void StartGimmick(NetworkRunner runner);
        public void EndGimmick(NetworkRunner runner);
    }
}