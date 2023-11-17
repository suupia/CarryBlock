using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyPassActionExecutor : IPassActionExecutor
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

        public void PassAction()
        {
            Debug.Log($"EmptyPassAction: Action");
        }
        
        public bool CanReceivePass()
        {
            return false;
        }

        public void PassBlock()
        {
            
        }
        
        public void ReceivePass(ICarriableBlock block)
        {
            
        }
        
        // Presenter
        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter)
        {
            
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            
        }
        
        public void SetPassBlockMoveExecutor(PassBlockMoveExecutorNet passBlockMoveExecutorNet)
        {
            
        }
    }
}