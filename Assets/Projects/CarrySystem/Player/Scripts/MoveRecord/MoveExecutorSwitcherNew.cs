﻿#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcherNew : IMoveExecutorSwitcherNew
    {
        readonly IList<IMoveRecord> _moveExecutors = new List<IMoveRecord>();
        PlayerInfo? _info;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        
        public void Move(Vector3 input)
        {
            // Aggregate IMoveParameter
            var regularMoveParameter = new RegularMoveParameter() as IMoveParameter;
            IMoveParameter parameter = _moveExecutors.Aggregate(regularMoveParameter, (integrated, next) => next.Chain(integrated));
            
            if(_info == null) throw new NullReferenceException("PlayerInfo is null");
            if(_playerAnimatorPresenter == null) throw new NullReferenceException("PlayerAnimatorPresenter is null" );
            
            // Aggregate IMoveFunction
            var regularMoveFunction = new RegularMoveFunction(_playerAnimatorPresenter, _info, parameter) as IMoveFunction;
            IMoveFunction function = _moveExecutors.Aggregate(regularMoveFunction, (integrated, next) => next.Chain(integrated));
            
            // Execute
            function.Move(input);
        }
        
        public void AddMoveRecord<T>() where T : IMoveRecord
        {
            var moveRecord = (T)Activator.CreateInstance(typeof(T), _playerAnimatorPresenter);
            _moveExecutors.Add(moveRecord);
        }
        
        public void RemoveRecord<T>() where T : IMoveRecord
        {
            var recordToRemove = _moveExecutors.OfType<T>().FirstOrDefault();
            if (recordToRemove != null)
            {
                _moveExecutors.Remove(recordToRemove);
            }
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }

}