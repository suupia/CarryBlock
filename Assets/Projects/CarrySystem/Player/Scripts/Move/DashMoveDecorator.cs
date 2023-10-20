using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashMoveDecorator : IMoveExecutor
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        readonly IMoveExecutorLeaf _moveExecutor;
        
        public DashMoveDecorator(IMoveExecutorLeaf moveExecutor)
        {
            _moveExecutor = moveExecutor.Clone();
            _moveExecutor.Acceleration *= 10.0f / 4.0f;
            _moveExecutor.MaxVelocity *= 10.0f / 5.0f;
            Debug.Log($"Construct _moveExecutor.MaxVelocity : {_moveExecutor.MaxVelocity}");

        }

        public void Setup(PlayerInfo info)
        {
            _moveExecutor.Setup(info);
        }

        public void Move(Vector3 input)
        {
            _moveExecutor.Move(input);
            Debug.Log($"_moveExecutor.MaxVelocity : {_moveExecutor.MaxVelocity}");


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