using Projects.CarrySystem.Block.Info;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate
    {
        BlockInfo Info { get; }
        void SetInfo(BlockInfo info);
    }
}