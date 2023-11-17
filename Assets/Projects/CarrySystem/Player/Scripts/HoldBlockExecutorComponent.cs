using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldBlockExecutorComponent : IHoldableComponent
    {
        public void Setup(PlayerInfo info)
        {
            
        }

        public void ResetHoldable()
        {
            
        }

        public bool TryToPickUpHoldable()
        {
            return false;
        }

        public bool TryToUseHoldable()
        {
            return false;
        }
        // View
        public void SetPlayerHoldablePresenter(PlayerAidKitPresenterNet presenter)
        {
            
        }
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            
        }

    }
}