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
        PlayerInfo _info;

        public MoveExecutorSwitcherNew(
            )
        {

        }

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
            Debug.Log($"_moveExecutors.Count: {_moveExecutors.Count}");
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
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchOffFaintedMove()
        {
            _moveExecutors.Remove(new FaintedMoveExecutorNew(_playerAnimatorPresenter));
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }

}