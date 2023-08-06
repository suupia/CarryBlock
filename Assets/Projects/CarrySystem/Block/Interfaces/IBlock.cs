using System.Collections.Generic;
using Carry.CarrySystem.Entity.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using UnityEngine;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface IBlock : IEntity
    {
        int MaxPlacedBlockCount { get; }
        bool CanPickUp();
        void PickUp();
        bool CanPutDown(IList<IBlock> blocks);
        void PutDown();
    }

    public enum BlockType
    {
        None,
        BasicBlock,
        UnmovableBlock,
        HeavyBlock,
    }
}