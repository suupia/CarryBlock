using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashMoveDecorator : IMoveExecutor
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        readonly IMoveExecutor _moveExecutor;
        
        public DashMoveDecorator(IMoveExecutorLeaf moveExecutor)
        {
            var acceleration = moveExecutor.Acceleration * 10.0f / 4.0f;
            var maxVelocity = moveExecutor.MaxVelocity * 10.0f / 5.0f;
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
            if (input != Vector3.zero)
            {
                _playerAnimatorPresenter?.Dash();   
            }
            else
            {
                _playerAnimatorPresenter?.Idle();
            }
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}