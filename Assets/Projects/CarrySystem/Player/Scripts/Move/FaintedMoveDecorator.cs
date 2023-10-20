using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class FaintedMoveDecorator : IMoveExecutor
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        readonly IMoveExecutor _moveExecutor;

        public FaintedMoveDecorator(RegularMoveExecutor moveExecutor)
        {
            var acceleration = 0;
            var maxVelocity = 0;
            var stoppingForce = moveExecutor.StoppingForce;
            _moveExecutor = new RegularMoveExecutor(acceleration, maxVelocity, stoppingForce);
        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutor.Setup(info);
        }

        public void Move(Vector3 input)
        {
            _moveExecutor.Move(input);

            // Todo : アニメーションの処理を無理やり上書きしているので、メソッドを切り出して修正する
            _playerAnimatorPresenter?.Idle();
        }

        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}