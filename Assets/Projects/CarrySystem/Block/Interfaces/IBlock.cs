using Carry.CarrySystem.Entity.Interfaces;

namespace Projects.CarrySystem.Block.Interfaces
{
    public interface IBlock : IEntity
    {
        bool BeingCarried { get; set; }
    }
}