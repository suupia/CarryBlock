using UnityEngine;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void HoldAction();

        public void ResetHoldingBlock();

        // View
        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter);
        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter);
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter);
    }
}