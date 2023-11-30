#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcher : IMoveExecutorSwitcher
    {
        readonly IList<IMoveChainable> _moveRecords = new List<IMoveChainable>();
        PlayerInfo? _info;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        
        public void Move(Vector3 input)
        {
            // Precondition
            Debug.Assert(_info != null, nameof(_info) + " != null");
            Debug.Assert(_playerAnimatorPresenter != null, nameof(_playerAnimatorPresenter) + " != null");
            
            // Aggregate IMoveParameter
            var regularMoveParameter = new RegularMoveParameter() as IMoveParameter;
            var parameter = _moveRecords.Aggregate(regularMoveParameter, (integrated, next) => next.Chain(integrated));
            
            // Aggregate IMoveFunction
            var regularMoveFunction = new RegularMoveFunction(_playerAnimatorPresenter, _info, parameter) as IMoveFunction;
            var function = _moveRecords.Aggregate(regularMoveFunction, (integrated, next) => next.Chain(integrated));
            
            // Execute
            function.Move(input);
        }
        
        public void AddMoveRecord<T>() where T : IMoveChainable
        {
            var moveRecord = (T)Activator.CreateInstance(typeof(T), _playerAnimatorPresenter);
            _moveRecords.Add(moveRecord);
        }
        
        public void RemoveRecord<T>() where T : IMoveChainable
        {
            var recordToRemove = _moveRecords.OfType<T>().FirstOrDefault();
            if (recordToRemove != null)
            {
                _moveRecords.Remove(recordToRemove);
            }
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }

}