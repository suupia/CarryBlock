using System;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    [RequireComponent(typeof(CarryPlayerControllerNet), typeof(Animator))]
    public class PlayerAnimatorPresenterNet : NetworkBehaviour, IPlayerAnimatorPresenter
    {
        Animator _animator;

        public struct PresentData : INetworkStruct
        {
            // [Networked] public PlayerBlockPresenterNet.BlockType HoldingBlockType { get; set; } // enumは共有できない(?)ので、int16で送る
        }

        public void Init(ICharacter character)
        {
            character.SetHoldPresenter(this);
        }

        public void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public override void Render()
        {
        }

        public void PickUpBlock(IBlock block)
        {
            _animator.SetTrigger("PickUp");
        }

        public void PutDownBlock()
        {
            _animator.SetTrigger("PutDown");
        }

        public void PassBlock()
        {
            _animator.SetTrigger("Pass");
        }

        public void ReceiveBlock(IBlock block)
        {
            _animator.SetTrigger("Receive");
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