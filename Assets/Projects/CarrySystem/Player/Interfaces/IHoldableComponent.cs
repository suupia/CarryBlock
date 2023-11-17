using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldableComponent
    {
        public void Setup(PlayerInfo info);
        public void ResetHoldable();
        public bool TryToPickUpHoldable();
        public bool TryToUseHoldable();
        // View
        public void SetPlayerHoldablePresenter(IPlayerHoldablePresenter presenter);
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter);
    }
}