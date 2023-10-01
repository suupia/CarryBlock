using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Projects.CarrySystem.Item.Interfaces;

#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity , IHighlightExecutor
    {
        IBlock? Block { get; }
        IList<IBlock> Blocks { get; }
        IList<IItem> Items { get; }
        public void AddBlock(IBlock block);

        public void RemoveBlock(IBlock block);

    }
}