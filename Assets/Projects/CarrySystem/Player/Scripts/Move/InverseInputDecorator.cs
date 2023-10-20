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
        public float Acceleration { get => _moveExecutorLeaf.Acceleration ;set => _moveExecutorLeaf.Acceleration = value;}
        public float MaxVelocity { get => _moveExecutorLeaf.MaxVelocity ;set => _moveExecutorLeaf.MaxVelocity = value; }
        public float StoppingForce {get => _moveExecutorLeaf.StoppingForce ;set => _moveExecutorLeaf.StoppingForce = value;}
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
        
        public IMoveExecutorLeaf Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (IMoveExecutorLeaf)MemberwiseClone();
        }

        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _moveExecutorLeaf.SetPlayerAnimatorPresenter(presenter);
        }
    }
}
