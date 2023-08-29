using Carry.CarrySystem.Entity.Interfaces;
using Projects.CarrySystem.Block.Info;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity
    {
        BlockInfo Info { get; }
        IBlock Block { get; }
        void SetInfo(BlockInfo info);
    }
}