#nullable enable

using System.Collections.Generic;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;

namespace Carry.CarrySystem.CarriableBlock.Interfaces
{
    public interface ICarriableBlock : IBlock
    {
        int MaxPlacedBlockCount { get; }
        bool CanPickUp();
        void PickUp(ICharacter character);
        bool CanPutDown(IList<ICarriableBlock> placedBlocks);
        void PutDown(ICharacter character);
    }
}