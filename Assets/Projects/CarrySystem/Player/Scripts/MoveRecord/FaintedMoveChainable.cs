﻿#nullable enable
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class FaintedMoveChainable : IMoveChainable 
    {
        readonly IPlayerAnimatorPresenter _playerAnimatorPresenter;
        public FaintedMoveChainable(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
        public IMoveParameter Chain(IMoveParameter parameter)
        {
            return new MoveParameter(parameter);
        }
        public IMoveFunction Chain(IMoveFunction function)
        {
            return new MoveFunction(function, _playerAnimatorPresenter);

        }
        class MoveParameter : IMoveParameter
        {
            public float Acceleration { get; } 
            public float MaxVelocity { get; } 
            public float StoppingForce { get;  }
            public MoveParameter(IMoveParameter parameter)
            {
                Acceleration = 0;
                MaxVelocity = 0;
                StoppingForce = parameter.StoppingForce;
            }
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

                _presenter.Idle();
            }
        }

    }
}