using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(AbstractNetworkPlayerController))]
    public class PlayerAnimatorPresenterNet : NetworkBehaviour, IPlayerAnimatorPresenter
    {
        public enum MovementState
        {
            Idle,
            InWalk,
            InDash,
        }
        public struct PresentData : INetworkStruct
        {
            public int PickUpCount { get; set; }
            public int PutDownCount { get; set; }
            public int ReceiveCount { get; set; }
            public int PassCount { get; set; }
            public  MovementState MovementState { get; set; }
            [Networked] public bool IsHoldingBlock { get; set; }
            public bool IsFainted { get; set; }
        }
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // 差分を判定して、アニメーションを発火させる
        int _pickUpCount;
        int _putDownCount;
        int _receiveCount;
        int _passCount;

        Animator? _animator;
        
        public void Init(IMoveExecutorSwitcherNew moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor, IOnDamageExecutor onDamageExecutor, IPassActionExecutor passActionExecutor)
        {
            Debug.Log($"PlayerAnimatorPresenterNet Init");
            moveExecutorSwitcher.SetPlayerAnimatorPresenter(this);
            holdActionExecutor.SetPlayerAnimatorPresenter(this);
            onDamageExecutor.SetPlayerAnimatorPresenter(this);
            passActionExecutor.SetPlayerAnimatorPresenter(this);
        }

        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }

        public override void Render()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            if (PresentDataRef.PickUpCount > _pickUpCount)
            {
                _animator.SetTrigger("PickUp");
                _pickUpCount = PresentDataRef.PickUpCount;
                Debug.Log($"PickUpCount: {PresentDataRef.PickUpCount},_pickUpCount: {_pickUpCount}");
            }
            if(PresentDataRef.PutDownCount > _putDownCount)
            {
                _animator.SetTrigger("PutDown");
                _putDownCount = PresentDataRef.PutDownCount;
                Debug.Log($"PutDownCount: {PresentDataRef.PutDownCount},_putDownCount: {_putDownCount}");
            }
            if(PresentDataRef.ReceiveCount > _receiveCount)
            {
                _animator.SetTrigger("Receive");
                _receiveCount = PresentDataRef.ReceiveCount;
                Debug.Log($"ReceiveCount: {PresentDataRef.ReceiveCount},_receiveCount: {_receiveCount}");
            }
            if(PresentDataRef.PassCount > _passCount)
            {
                _animator.SetTrigger("Pass");
                _passCount = PresentDataRef.PassCount;
                Debug.Log($"PassCount: {PresentDataRef.PassCount},_passCount: {_passCount}");
            }
            if(PresentDataRef.IsFainted)
            {
                _animator.SetBool("IsFainted",true);
                //Debug.Log($"IsFainted: {PresentDataRef.IsFainted}");
            }
            else
            {
                _animator.SetBool("IsFainted",false);
            }

            switch (PresentDataRef.MovementState)
            {
                case MovementState.Idle:
                    _animator.SetBool("InWalk", false);
                    _animator.SetBool("InDash", false);
                    break;
                case MovementState.InWalk:
                    _animator.SetBool("InWalk", true);
                    _animator.SetBool("InDash", false);
                    break;
                case MovementState.InDash:
                    _animator.SetBool("InWalk", false);
                    _animator.SetBool("InDash", true);
                    break;
            }
            _animator.SetBool("IsHoldingBlock", PresentDataRef.IsHoldingBlock);
        }

        public void PickUpBlock(IBlock block)
        {
            PresentDataRef.PickUpCount++;
            PresentDataRef.IsHoldingBlock = true;
        }

        public void PutDownBlock()
        {
            PresentDataRef.PutDownCount++;
            PresentDataRef.IsHoldingBlock = false;
        }
        
        public void ReceiveBlock(IBlock block)
        {
            PresentDataRef.ReceiveCount++;
            PresentDataRef.IsHoldingBlock = true;
        }

        public void PassBlock()
        {
            PresentDataRef.PassCount++;
            PresentDataRef.IsHoldingBlock = false;
        }


        public void Idle()
        {
            PresentDataRef.MovementState = MovementState.Idle;
        }

        public void Walk()
        {
            PresentDataRef.MovementState = MovementState.InWalk;
        }

        public void Dash()
        {
            PresentDataRef.MovementState = MovementState.InDash;
        }
        
        public void Faint()
        {
            PresentDataRef.IsFainted = true;
        }
        public void Revive()
        {
            PresentDataRef.IsFainted = false;
        }
    }
}