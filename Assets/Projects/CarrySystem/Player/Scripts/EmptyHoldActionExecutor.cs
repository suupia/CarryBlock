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

        public void HoldAction()
        {
            Debug.Log($"EmptyHoldAction: Action");
        }
        
        public void SetPlayerAidKitPresenter(PlayerAidKitPresenterNet presenter)
        {
            
        }
    }
}