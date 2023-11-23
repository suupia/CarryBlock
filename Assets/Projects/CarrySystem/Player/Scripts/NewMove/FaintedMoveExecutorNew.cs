﻿#nullable enable
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;


namespace Carry.CarrySystem.Player.Scripts
{
    public class FaintedMoveExecutorNew : IMoveExecutorNew 
    {
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;
        
        public IMoveParameter Chain(IMoveParameter parameter)
        {
            return new ReturnParameter(parameter);
        }
        class ReturnParameter : IMoveParameter
        {
            public float Acceleration { get; set; } = 0;
            public float MaxVelocity { get; set; } = 0;
            public float StoppingForce { get; set; }
            public ReturnParameter(IMoveParameter parameter)
            {
                StoppingForce = parameter.StoppingForce;
            }
        }
        
        public IMoveFunction Chain(IMoveFunction function)
        {
            return new ReturnFunction(function, _playerAnimatorPresenter);

        }
        
        class ReturnFunction : IMoveFunction
        {
            readonly IMoveFunction _func;
            readonly IPlayerAnimatorPresenter _presenter;
            public ReturnFunction(IMoveFunction function, IPlayerAnimatorPresenter presenter)
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
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}