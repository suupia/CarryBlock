using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyHoldActionExecutor : IHoldActionExecutor
    {
        public void SetHoldPresenter(IPlayerHoldablePresenter presenter)
        {
            
        }

        public void Setup(PlayerInfo info)
        {
        }

        public void Reset()
        {
            
        }

        public void ResetHoldingBlock()
        {
            
        }

        public void HoldAction()
        {
            Debug.Log($"EmptyHoldAction: Action");
        }
        
        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
            
        }
        
        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            
        }
        
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            
        }
    }
}