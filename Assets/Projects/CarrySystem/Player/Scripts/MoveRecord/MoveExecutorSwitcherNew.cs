using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcherNew : IMoveExecutorSwitcherNew
    {
        IPlayerAnimatorPresenter _playerAnimatorPresenter = null!;
        readonly IList<IMoveRecord> _moveExecutors = new List<IMoveRecord>();
        PlayerInfo? _info;
        

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        
        public void Move(Vector3 input)
        {
            var regularMoveParameter = new RegularMoveParameter() as IMoveParameter;
            IMoveParameter parameter = _moveExecutors.Aggregate(regularMoveParameter, (integrated, next) => next.Chain(integrated));
            var regularMoveFunction = new RegularMoveFunction(_playerAnimatorPresenter, _info, parameter) as IMoveFunction;
            IMoveFunction function = _moveExecutors.Aggregate(regularMoveFunction, (integrated, next) => next.Chain(integrated));
            function.Move(input);
        }

        
        public void SwitchToConfusionMove()
        {
            _moveExecutors.Add(new InverseInputMoveRecord(_playerAnimatorPresenter));
        }
        
        public void SwitchOffConfusionMove()
        {
            _moveExecutors.Remove(new InverseInputMoveRecord(_playerAnimatorPresenter));
        }
        
        public void SwitchToDashMove()
        {
            var nextMoveExe = new DashMoveRecord(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffDashMove()
        {
            _moveExecutors.Remove(new DashMoveRecord(_playerAnimatorPresenter));
        }
        public void SwitchToSlowMove()
        {
            var nextMoveExe = new SlowMoveRecord(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffSlowMove()
        {
            _moveExecutors.Remove(new SlowMoveRecord(_playerAnimatorPresenter));
        }
        public void SwitchToFaintedMove()
        {
            var nextMoveExe = new FaintedMoveRecord(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffFaintedMove()
        {
            _moveExecutors.Remove(new FaintedMoveRecord(_playerAnimatorPresenter));
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