﻿#nullable enable
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class ConfusionMoveChainable : IMoveChainable
    {
        public ConfusionMoveChainable(IPlayerAnimatorPresenter _)
        {
        }

        public IMoveParameter Chain(IMoveParameter parameter)
        {
            return parameter;
        }
        
        public IMoveFunction Chain(IMoveFunction function)
        {
            return new MoveFunction(function);
        }
        
        class MoveFunction : IMoveFunction
        {
            readonly IMoveFunction _func;
            public MoveFunction(IMoveFunction function)
            {
                _func = function; 
            }
          
            public void Move(Vector3 input)
            {
                var reverseInput = new Vector3(-input.x, input.y, -input.z);
                _func.Move(reverseInput);
            }
        }
    }
}