using System.Collections.Generic;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcher : IMoveExecutorSwitcher
    {
        public  IMoveExecutor CurrentMoveExecutor => _currentMoveExecutor;
        readonly IMoveExecutor _regularMoveExecutor;
        readonly IMoveExecutor _slowMoveExecutor;
        readonly IMoveExecutor _confusionMoveExecutor;
        readonly IMoveExecutor _dashMoveExecutor;
        readonly IMoveExecutor _faintedMoveExecutor;
        IMoveExecutor _beforeMoveExecutor;
        IMoveExecutor _currentMoveExecutor;
        
        public MoveExecutorSwitcher(
            )
        {
            _regularMoveExecutor = new CorrectlyStopMoveExecutor();
            _slowMoveExecutor = new SlowMoveExecutor();
            _confusionMoveExecutor = new ConfusionMoveExecutor(_regularMoveExecutor);
            _dashMoveExecutor = new DashMoveExecutor();
            _faintedMoveExecutor = new FaintedMoveExecutor();
            _currentMoveExecutor = _regularMoveExecutor;
            _beforeMoveExecutor = _regularMoveExecutor;
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveExecutor.Setup(info);
            _slowMoveExecutor.Setup(info);
            _dashMoveExecutor.Setup(info);
            _faintedMoveExecutor.Setup(info);
        }
        
        public void Move(Vector3 input)
        {
            _currentMoveExecutor.Move(input);
        }
        
        public void SwitchToBeforeMoveExecutor()
        {
            HoldBeforeMove();
            _currentMoveExecutor =  _beforeMoveExecutor;
        }
        
        public void SwitchToRegularMove()
        {
            HoldBeforeMove();
            _currentMoveExecutor =  _regularMoveExecutor;
        }
        
        public void SwitchToDashMove()
        {
            HoldBeforeMove();
            _currentMoveExecutor =  _dashMoveExecutor;
        }
        public void SwitchToSlowMove()
        {
            HoldBeforeMove();
            _currentMoveExecutor =  _slowMoveExecutor;
        }
        public void SwitchToConfusionMove()
        {
            HoldBeforeMove();
            _currentMoveExecutor =  _confusionMoveExecutor;
        }
        public void SwitchToFaintedMove()
        {           
            HoldBeforeMove();
            _currentMoveExecutor =  _faintedMoveExecutor;
        }
        
        void HoldBeforeMove()
        {
            if (_currentMoveExecutor is CorrectlyStopMoveExecutor ||
                _currentMoveExecutor is SlowMoveExecutor)
            {
                _beforeMoveExecutor = _currentMoveExecutor;
            }
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _regularMoveExecutor.SetPlayerAnimatorPresenter(presenter);
            _slowMoveExecutor.SetPlayerAnimatorPresenter(presenter);
            _dashMoveExecutor.SetPlayerAnimatorPresenter(presenter);
            _faintedMoveExecutor.SetPlayerAnimatorPresenter(presenter);
        }
    }
}