using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashMoveExecutor : IMoveExecutor ,IMoveFunctionOld
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        readonly IMoveExecutorLeaf _moveExecutor;
        
        public DashMoveExecutor(IMoveExecutorLeaf moveExecutorLeaf)
        {
            _moveExecutor = moveExecutorLeaf.CreateNewLeaf();
            _moveExecutor.Acceleration *= 10.0f / 4.0f;
            _moveExecutor.MaxVelocity *= 10.0f / 5.0f;
            Debug.Log($"Construct _moveExecutor.MaxVelocity : {_moveExecutor.MaxVelocity}");

        }
        
        public IMoveExecutorLeaf Chain(IMoveExecutorLeaf moveExecutorLeaf)
        {
            var newMove = moveExecutorLeaf.CreateNewLeaf();
            newMove.Acceleration *= 10.0f / 4.0f;
            newMove.MaxVelocity *= 10.0f / 5.0f;
            return newMove;
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