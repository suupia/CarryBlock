using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Entity.Interfaces;
using Projects.CarrySystem.Item.Scripts;

namespace Projects.CarrySystem.Item.Interfaces
{
    public interface IItem : IPlaceable
    {
        public void OnGained(ItemControllerNet controller);
    }
}