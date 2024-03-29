﻿using Carry.CarrySystem.Block.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerAnimatorPresenter
    {
        public void Init(IMoveExecutorSwitcher moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor,
            IOnDamageExecutor onDamageExecutor, IPassActionExecutor passActionExecutor);
        public void SetAnimator(Animator animator);
        void PickUpBlock(IBlock block);
        void PutDownBlock();
        void ReceiveBlock(IBlock block);
        void PassBlock();
        void Idle();
        void Walk();
        void Dash();
        void Faint();
        void Revive();
    }
}