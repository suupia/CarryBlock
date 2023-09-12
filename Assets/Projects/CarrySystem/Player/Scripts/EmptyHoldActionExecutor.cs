using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyHoldActionExecutor : IHoldActionExecutor
    {
        public void SetHoldPresenter(IPlayerBlockPresenter presenter)
        {
            
        }

        public void Setup(PlayerInfo info)
        {
        }

        public void Reset()
        {
            
        }

        public void PutDownBlock()
        {
            
        }

        public void HoldAction()
        {
            Debug.Log($"EmptyHoldAction: Action");
        }
        
        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter)
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