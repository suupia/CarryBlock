#nullable enable
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashMoveExecutorNew : IMoveExecutorNew
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public IMoveParameter Chain(IMoveParameter parameter)
        {
            var newMove = parameter;
            newMove.Acceleration *= 10.0f / 4.0f;
            newMove.MaxVelocity *= 10.0f / 5.0f;
            return newMove;
        }
        
        public IMoveFunction Chain(IMoveFunction function)
        {
            return new MoveFunction(function, _playerAnimatorPresenter);

        }
        
        class MoveFunction : IMoveFunction
        {
            readonly IMoveFunction _func;
            readonly IPlayerAnimatorPresenter _presenter;
            public MoveFunction(IMoveFunction function, IPlayerAnimatorPresenter presenter)
            {
                _func = function;
                _presenter = presenter;
            }
            public void Move(Vector3 input)
            {
                _func.Move(input);

                if (input != Vector3.zero)
                {
                    _presenter.Dash();   
                }
                else
                {
                    _presenter.Idle();
                }
            }
        }

        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}