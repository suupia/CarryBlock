using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Gimmick.Interfaces;
using Projects.CarrySystem.Item.Interfaces;

#nullable enable

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlockMonoDelegate : IEntity
    {
        IBlock? Block { get; }
        public void AddBlock(IBlock block);

        public void RemoveBlock(IBlock block);

    }
}