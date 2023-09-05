using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity , IHighlightExecutor
    {
        ICarriableBlock? Block { get; }
        IList<ICarriableBlock> Blocks { get; }
        public void AddBlock(ICarriableBlock block);

        public void RemoveBlock(ICarriableBlock block);

    }
}