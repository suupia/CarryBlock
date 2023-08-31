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
            [Networked] public int PickUpCount { get; set; }
            [Networked] public int PutDownCount { get; set; }
            [Networked] public int ReceiveCount { get; set; }
            [Networked] public int PassCount { get; set; }
            [Networked] public  MovementState MovementState { get; set; }
            [Networked] public bool IsHoldingBlock { get; set; }
        }
        [Networked] public ref PresentData PresentDataRef => ref MakeRef<PresentData>();

        // 差分を判定して、アニメーションを発火させる
        int _pickUpCount;
        int _putDownCount;
        int _receiveCount;
        int _passCount;

        Animator? _animator;
        
        public void Init(ICharacter character)
        {
            Debug.Log($"PlayerAnimatorPresenterNet Init");
            character.SetHoldPresenter(this);
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
            }
            if(PresentDataRef.PutDownCount > _putDownCount)
            {
                _animator.SetTrigger("PutDown");
                _putDownCount = PresentDataRef.PutDownCount;
            }
            if(PresentDataRef.ReceiveCount > _receiveCount)
            {
                _animator.SetTrigger("Receive");
                _receiveCount = PresentDataRef.ReceiveCount;
            }
            if(PresentDataRef.PassCount > _passCount)
            {
                _animator.SetTrigger("Pass");
                _passCount = PresentDataRef.PassCount;
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
            PresentDataRef.IsHoldingBlock = false;
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
    }
}