#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutorComponent
    {
        public bool TryToPickUp();
        public bool TryToPutDown();
    }
}