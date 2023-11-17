using Carry.CarrySystem.Block.Interfaces;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerBlockPresenter
    {
        public void EnableHoldableView(IHoldable holdable);
        public void DisableHoldableView();

    }
}