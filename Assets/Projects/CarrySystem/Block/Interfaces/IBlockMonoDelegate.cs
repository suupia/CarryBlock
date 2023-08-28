using Projects.CarrySystem.Block.Info;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IBlock
    {
        BlockInfo Info { get; }
        void SetInfo(BlockInfo info);
    }
}