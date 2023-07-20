using Carry.CarrySystem.Entity.Interfaces;

namespace Projects.CarrySystem.Block.Interfaces
{
    public interface IBlock : IEntity
    {
        int MaxPlacedBlockCount { get; }
        bool BeingCarried { get; set; }
    }
}