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
        Animator? _animator;

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
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetTrigger("PickUp");
            _animator.SetBool("IsHoldingBlock", true);
        }

        public void PutDownBlock()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetTrigger("PutDown");
            _animator.SetBool("IsHoldingBlock", false);
        }
        
        public void ReceiveBlock(IBlock block)
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetTrigger("Receive");
            _animator.SetBool("isHoldingBlock", false);
        }

        public void PassBlock()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetTrigger("Pass");
            _animator.SetBool("isHoldingBlock", false);
        }


        public void Idle()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetBool("InWalk", false);
            _animator.SetBool("InDash", false);
        }

        public void Walk()
        {
            Debug.Log($"Walk Animation");
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetBool("InWalk", true);
            _animator.SetBool("InDash", false);
        }

        public void Dash()
        {
            if (_animator == null)
            {
                Debug.LogError($"_animator is null");
                return;
            }
            _animator.SetBool("InWalk", false);
            _animator.SetBool("InDash", true);
        }
    }
}