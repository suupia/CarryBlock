using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class InverseInputDecorator : IMoveExecutorLeaf
    {
        public float Acceleration => _moveExecutorLeaf.Acceleration;
        public float MaxVelocity => _moveExecutorLeaf.MaxVelocity;
        public float StoppingForce => _moveExecutorLeaf.StoppingForce;
        readonly IMoveExecutorLeaf _moveExecutorLeaf;

        public InverseInputDecorator(IMoveExecutorLeaf moveExecutorLeaf)
        {
            _moveExecutorLeaf = moveExecutorLeaf;
        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutorLeaf.Setup(info);
        }

        public void Move(Vector3 input)
        {
            var reverseInput = new Vector3(-input.x, input.y, -input.z);
            _moveExecutorLeaf.Move(reverseInput);
        }

        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _moveExecutorLeaf.SetPlayerAnimatorPresenter(presenter);
        }
    }
}
