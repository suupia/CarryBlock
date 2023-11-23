using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerControllerLocal))]
    public class PlayerAnimatorPresenterLocal : MonoBehaviour, IPlayerAnimatorPresenter
    {
        public enum MovementState
        {
            Idle,
            InWalk,
            InDash,
        }
        public struct PresentData
        {
            public int PickUpCount { get; set; }
            public int PutDownCount { get; set; }
            public int ReceiveCount { get; set; }
            public int PassCount { get; set; }
            public  MovementState MovementState { get; set; }
            public bool IsHoldingBlock { get; set; }
            public bool IsFainted { get; set; }
        }
         PresentData _presentData;

        // 差分を判定して、アニメーションを発火させる
        int _pickUpCount;
        int _putDownCount;
        int _receiveCount;
        int _passCount;

        Animator? _animator;
        
        public void Init(IMoveExecutorSwitcher moveExecutorSwitcher, IHoldActionExecutor holdActionExecutor, IOnDamageExecutor onDamageExecutor, IPassActionExecutor passActionExecutor)
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

         void Update()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            if (_presentData.PickUpCount > _pickUpCount)
            {
                _animator.SetTrigger("PickUp");
                _pickUpCount = _presentData.PickUpCount;
                Debug.Log($"PickUpCount: {_presentData.PickUpCount},_pickUpCount: {_pickUpCount}");
            }
            if(_presentData.PutDownCount > _putDownCount)
            {
                _animator.SetTrigger("PutDown");
                _putDownCount = _presentData.PutDownCount;
                Debug.Log($"PutDownCount: {_presentData.PutDownCount},_putDownCount: {_putDownCount}");
            }
            if(_presentData.ReceiveCount > _receiveCount)
            {
                _animator.SetTrigger("Receive");
                _receiveCount = _presentData.ReceiveCount;
                Debug.Log($"ReceiveCount: {_presentData.ReceiveCount},_receiveCount: {_receiveCount}");
            }
            if(_presentData.PassCount > _passCount)
            {
                _animator.SetTrigger("Pass");
                _passCount = _presentData.PassCount;
                Debug.Log($"PassCount: {_presentData.PassCount},_passCount: {_passCount}");
            }
            if(_presentData.IsFainted)
            {
                _animator.SetBool("IsFainted",true);
                //Debug.Log($"IsFainted: {PresentDataRef.IsFainted}");
            }
            else
            {
                _animator.SetBool("IsFainted",false);
            }

            switch (_presentData.MovementState)
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
            _animator.SetBool("IsHoldingBlock", _presentData.IsHoldingBlock);
        }

        public void PickUpBlock(IBlock block)
        {
            _presentData.PickUpCount++;
            _presentData.IsHoldingBlock = true;
        }

        public void PutDownBlock()
        {
            _presentData.PutDownCount++;
            _presentData.IsHoldingBlock = false;
        }
        
        public void ReceiveBlock(IBlock block)
        {
            _presentData.ReceiveCount++;
            _presentData.IsHoldingBlock = true;
        }

        public void PassBlock()
        {
            _presentData.PassCount++;
            _presentData.IsHoldingBlock = false;
        }


        public void Idle()
        {
            _presentData.MovementState = MovementState.Idle;
        }

        public void Walk()
        {
            _presentData.MovementState = MovementState.InWalk;
        }

        public void Dash()
        {
            _presentData.MovementState = MovementState.InDash;
        }
        
        public void Faint()
        {
            _presentData.IsFainted = true;
        }
        public void Revive()
        {
            _presentData.IsFainted = false;
        }
    }
}