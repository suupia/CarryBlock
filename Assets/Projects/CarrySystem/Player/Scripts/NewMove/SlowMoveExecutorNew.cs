#nullable enable
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class SlowMoveExecutorNew : IMoveExecutorNew
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        
        public IMoveParameter Chain(IMoveParameter parameter)
        {
            return new MoveParameter(parameter);
        } 
        
        class MoveParameter : IMoveParameter
        {
            public float Acceleration { get;  }
            public float MaxVelocity { get;  }
            public float StoppingForce { get;  }

            public MoveParameter(IMoveParameter parameter)
            {
                Acceleration = parameter.Acceleration * 3.0f / 4.0f;
                MaxVelocity = parameter.MaxVelocity * 2.0f / 5.0f;
                StoppingForce = parameter.StoppingForce;
            }
           
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
                    _presenter.Walk();   
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