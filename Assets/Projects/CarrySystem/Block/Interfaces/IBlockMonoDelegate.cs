using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity , IHighlightExecutor
    {
        IBlock? Block { get; }
        IList<IBlock> Blocks { get; }
        
        public void OnStart();
        public void OnEnd();
        public void AddBlock(IBlock block);

        public void RemoveBlock(IBlock block);

    }
}