using Carry.CarrySystem.Block.Interfaces;
#nullable enable
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerHoldablePresenter
    {
        public void Init(IHoldActionExecutor holdActionExecutor, IPassActionExecutor passActionExecutor);

        public void EnableHoldableView(IHoldable holdable);
        public void DisableHoldableView();

    }
}