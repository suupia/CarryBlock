using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcherNew : IMoveExecutorSwitcher
    {
        PlayerInfo _info = null!;
        IPlayerAnimatorPresenter _playerAnimatorPresenter = null!;
        readonly IList<IMoveExecutorNew> _moveExecutors = new List<IMoveExecutorNew>();

        public MoveExecutorSwitcherNew(
            )
        {
            var regularMoveExecutor = new RegularMoveExecutorNew(40, 5, 5);
            _moveExecutors.Add(regularMoveExecutor);
        }

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        
        public void Move(Vector3 input)
        {
            IMoveParameter parameter = _moveExecutors.Aggregate((IMoveParameter)null, (integrated, next) => next.Chain(integrated));
            IMoveFunction function = _moveExecutors.Aggregate((IMoveFunction)null, (integrated, next) => next.Chain(integrated));
            MoveExecutorNew moveExecutorNew = new MoveExecutorNew();
            moveExecutorNew.Move(input, parameter, function);
            
        }
        
        public void SwitchToBeforeMoveExecutor()
        {
           // todo : メソッドを分割して、Remove処理を書く
        }
        
        public void SwitchToRegularMove()
        {
            // todo : メソッドを分割して、Remove処理を書く
        }
        
        public void SwitchToConfusionMove()
        {
            _moveExecutors.Add(new InverseInputMoveExecutorNew());
        }
        
        public void SwitchToDashMove()
        {
            var nextMoveExe = new DashMoveExecutorNew(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }
        public void SwitchToSlowMove()
        {
            var nextMoveExe = new SlowMoveExecutorNew(_playerAnimatorPresenter);
            _moveExecutors.Add(nextMoveExe);
        }

        public void SwitchToFaintedMove()
        {
            var nextMoveExe = new FaintedMoveExecutorNew(_playerAnimatorPresenter);
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }

}