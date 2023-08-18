using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerControllerNet))]
    public class PlayerAnimatorPresenterNet : NetworkBehaviour, IPlayerAnimatorPresenter
    {
        Animator _animator;

        public struct PresentData : INetworkStruct
        {
            // [Networked] public PlayerBlockPresenterNet.BlockType HoldingBlockType { get; set; } // enumは共有できない(?)ので、int16で送る
        }

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
        }

        public void PickUpBlock(IBlock block)
        {
            Debug.Log($"SetTrigger PickUp");
            _animator.SetTrigger("PickUp");
            _animator.SetBool("isHoldingBlock", true);
        }

        public void PutDownBlock()
        {
            _animator.SetTrigger("PutDown");
            _animator.SetBool("isHoldingBlock", false);
        }
        
        public void ReceiveBlock(IBlock block)
        {
            _animator.SetTrigger("Receive");
            _animator.SetBool("isHoldingBlock", false);
        }

        public void PassBlock()
        {
            _animator.SetTrigger("Pass");
            _animator.SetBool("isHoldingBlock", false);
        }


        public void Idle()
        {
            _animator.SetBool("InWalk", false);
            _animator.SetBool("InDash", false);
        }

        public void Walk()
        {
            _animator.SetBool("InWalk", true);
            _animator.SetBool("InDash", false);
        }

        public void Dash()
        {
            _animator.SetBool("InWalk", false);
            _animator.SetBool("InDash", true);
        }
    }
}