using System.Collections;
using System.Collections.Generic;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class InverseInputDecorator : IMoveExecutor
    {
        readonly IMoveExecutor _moveExecutor;

        public InverseInputDecorator(IMoveExecutor moveExecutor)
        {
            _moveExecutor = moveExecutor;
        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutor.Setup(info);
        }

        public void Move(Vector3 input)
        {
            var reverseInput = new Vector3(-input.x, input.y, -input.z);
            _moveExecutor.Move(reverseInput);
        }

        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _moveExecutor.SetPlayerAnimatorPresenter(presenter);
        }
    }
}
