﻿using System.Collections.Generic;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorContainer : IMoveExecutorContainer
    {
        public  IMoveExecutor CurrentMoveExecutor => _currentMoveExecutor;
        readonly IMoveExecutor _regularMoveExecutor;
        readonly IMoveExecutor _slowMoveExecutor;
        IMoveExecutor _currentMoveExecutor;
        
        public MoveExecutorContainer(
            )
        {
            _regularMoveExecutor = new CorrectlyStopMoveExecutor();
            _slowMoveExecutor = new SlowMoveExecutor();
            _currentMoveExecutor = _regularMoveExecutor;
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveExecutor.Setup(info);
            _slowMoveExecutor.Setup(info);
        }
        
        public void Move(Vector3 input)
        {
            _currentMoveExecutor.Move(input);
        }
        
        public void SetRegularMoveExecutor()
        {
            _currentMoveExecutor =  _regularMoveExecutor;
        }
        public void SetSlowMoveExecutor()
        {
            _currentMoveExecutor =  _slowMoveExecutor;
        }
    }
}