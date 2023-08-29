using Projects.CarrySystem.Block.Info;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IBlock
    {
        BlockInfo Info { get; }
        IBlock Block { get; }
        void SetInfo(BlockInfo info);
    }
}