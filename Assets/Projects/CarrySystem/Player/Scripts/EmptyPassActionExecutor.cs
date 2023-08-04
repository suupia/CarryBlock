using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyPassActionExecutor : IPassActionExecutor
    {
        // public void SetHoldPresenter(IHoldActionPresenter presenter)
        // {
        //     
        // }

        public void Setup(PlayerInfo info)
        {
        }

        public void Reset()
        {
            
        }

        public void PassAction()
        {
            Debug.Log($"EmptyPassAction: Action");
        }
    }
}