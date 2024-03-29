﻿using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPassActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void PassAction();

        bool CanReceivePass();

        void ReceivePass(ICarriableBlock block);
        
        // View
        public void SetPlayerBlockPresenter(IPlayerHoldablePresenter presenter);
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter);
        
        public void SetPassBlockMoveExecutor(PassBlockMoveExecutorNet passBlockMoveExecutorNet);
    }
}