using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class SlowMoveExecutor : IMoveExecutor ,IMoveFunction
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        readonly IMoveExecutorLeaf _moveExecutor;
        
        public SlowMoveExecutor(IMoveExecutorLeaf moveExecutorLeaf)
        {
            var acceleration = moveExecutorLeaf.Acceleration * 3.0f / 4.0f;
            var maxVelocity = moveExecutorLeaf.MaxVelocity * 2.0f / 5.0f;
            var stoppingForce = moveExecutorLeaf.StoppingForce;
            _moveExecutor = new RegularMoveExecutor(acceleration, maxVelocity, stoppingForce);
        }
        
        public IMoveExecutorLeaf Chain(IMoveExecutorLeaf moveExecutorLeaf)
        {
            var acceleration = moveExecutorLeaf.Acceleration * 3.0f / 4.0f;
            var maxVelocity = moveExecutorLeaf.MaxVelocity * 2.0f / 5.0f;
            var stoppingForce = moveExecutorLeaf.StoppingForce;
            return new RegularMoveExecutor(acceleration, maxVelocity, stoppingForce);
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
                _playerAnimatorPresenter?.Walk();   
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