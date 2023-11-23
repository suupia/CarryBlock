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
        readonly IMoveExecutorLeaf _regularMoveLeaf;
        readonly IMoveExecutorLeaf _confusionMoveLeaf;
        
        IMoveExecutor _currentMoveExecutor;
        IMoveExecutorLeaf _beforeMoveExecutorLeaf;
        
        PlayerInfo _info = null!;
        IPlayerAnimatorPresenter _playerAnimatorPresenter = null!;
        readonly IList<IMoveExecutorNew> _moveExecutors = new List<IMoveExecutorNew>();

        public MoveExecutorSwitcherNew(
            )
        {
            var regularMoveExecutor = new RegularMoveExecutor(40, 5, 5);
            _regularMoveLeaf = regularMoveExecutor;
            _confusionMoveLeaf = new InverseInputMoveExecutorLeaf(regularMoveExecutor);
            _beforeMoveExecutorLeaf = regularMoveExecutor;
            _currentMoveExecutor = _regularMoveLeaf;
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveLeaf.Setup(info);
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
            // Debug.Log($"_beforeMoveExecutorNew.GetType() : {_beforeMoveExecutorLeaf.GetType()}");
            _currentMoveExecutor = _beforeMoveExecutorLeaf;
        }
        
        public void SwitchToRegularMove()
        {
            _currentMoveExecutor =  _regularMoveLeaf;
            _beforeMoveExecutorLeaf = _regularMoveLeaf;
        }
        
        public void SwitchToConfusionMove()
        {
            _currentMoveExecutor =  _confusionMoveLeaf;
            _beforeMoveExecutorLeaf = _confusionMoveLeaf;
        }
        
        public void SwitchToDashMove()
        {
            if (_currentMoveExecutor is IMoveExecutorLeaf moveExecutorLeaf)
            {
                _beforeMoveExecutorLeaf = moveExecutorLeaf;
            }
            var nextMoveExe = new DashMoveExecutor(_beforeMoveExecutorLeaf);
            nextMoveExe.Setup(_info);
            nextMoveExe.SetPlayerAnimatorPresenter(_playerAnimatorPresenter);
            _currentMoveExecutor = nextMoveExe;
        }
        public void SwitchToSlowMove()
        {
            if (_currentMoveExecutor is IMoveExecutorLeaf moveExecutorLeaf)
            {
                _beforeMoveExecutorLeaf = moveExecutorLeaf;
            }
            var nextMoveExe = new SlowMoveExecutor(_beforeMoveExecutorLeaf);
            nextMoveExe.Setup(_info);
            nextMoveExe.SetPlayerAnimatorPresenter(_playerAnimatorPresenter);
            _currentMoveExecutor = nextMoveExe;
        }

        public void SwitchToFaintedMove()
        {
            if (_currentMoveExecutor is IMoveExecutorLeaf moveExecutorLeaf)
            {
                _beforeMoveExecutorLeaf = moveExecutorLeaf;
            }
            var nextMoveExe = new FaintedMoveExecutor(_beforeMoveExecutorLeaf);
            nextMoveExe.Setup(_info);
            nextMoveExe.SetPlayerAnimatorPresenter(_playerAnimatorPresenter);
            _currentMoveExecutor = nextMoveExe;
        }
        
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _regularMoveLeaf.SetPlayerAnimatorPresenter(presenter);
            _playerAnimatorPresenter = presenter;
        }
    }

}