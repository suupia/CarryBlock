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
        readonly IList<IMoveExecutorNew> _moveExecutors = new List<IMoveExecutorNew>();
        readonly RegularMoveExecutorNew _regularMoveExecutorNew;

        public MoveExecutorSwitcherNew(
            )
        {
            _regularMoveExecutorNew = new RegularMoveExecutorNew(40, 5, 5);
            _moveExecutors.Add(_regularMoveExecutorNew);
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveExecutorNew.SetUp(info);
        }
        
        public void Move(Vector3 input)
        {
            IMoveParameter parameter = _moveExecutors.Aggregate(new EmptyMove() as IMoveParameter, (integrated, next) => next.Chain(integrated));
            IMoveFunction function = _moveExecutors.Aggregate(new EmptyMove() as IMoveFunction, (integrated, next) => next.Chain(integrated));
            MoveExecutorNew moveExecutorNew = new MoveExecutorNew();
            moveExecutorNew.Move(input, parameter, function);
            
        }
        
        public void SwitchToConfusionMove()
        {
            _moveExecutors.Add(new InverseInputMoveExecutorNew());
        }
        
        public void SwitchOffConfusionMove()
        {
            _moveExecutors.Remove(new InverseInputMoveExecutorNew());
        }
        
        public void SwitchToDashMove()
        {
            var nextMoveExe = new DashMoveExecutorNew(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffDashMove()
        {
            _moveExecutors.Remove(new DashMoveExecutorNew(_playerAnimatorPresenter));
        }
        public void SwitchToSlowMove()
        {
            var nextMoveExe = new SlowMoveExecutorNew(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffSlowMove()
        {
            _moveExecutors.Remove(new SlowMoveExecutorNew(_playerAnimatorPresenter));
        }
        public void SwitchToFaintedMove()
        {
            var nextMoveExe = new FaintedMoveExecutorNew(_playerAnimatorPresenter);
        }
        public void SwitchOffFaintedMove()
        {
            _moveExecutors.Remove(new FaintedMoveExecutorNew(_playerAnimatorPresenter));
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
            _regularMoveExecutorNew.SetPlayerAnimatorPresenter(presenter);
        }
    }

}