
#nullable enable
using Carry.CarrySystem.Block.Info;
using Fusion;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMaterialSetter
    {
        public void Init(BlockInfo info);

        public void ChangeWhite(PlayerRef playerRef);
    }
}