using Carry.CarrySystem.Entity.Interfaces;

namespace Carry.CarrySystem.Block.Interfaces
{
    // what is placed in the game
    // Needed to differentiate from "Ground"
    public interface IPlaceable : IEntity
    {
        int MaxPlacedBlockCount { get; }
    }
}