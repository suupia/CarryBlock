#nullable enable

using System.Collections.Generic;
using Carry.CarrySystem.Player.Interfaces;

namespace Carry.CarrySystem.Block.Interfaces
{
    public interface ICarriableBlock : IBlock
    {
        int MaxPlacedBlockCount { get; }
        bool CanPickUp();
        void PickUp(ICharacter character);
        bool CanPutDown(IList<ICarriableBlock> blocks);
        void PutDown(ICharacter character);
    }
}