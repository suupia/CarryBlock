using System.Collections.Generic;
using System.Security.Cryptography;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MoveExecutorSwitcher : IMoveExecutorSwitcher
    {
        public  IMoveExecutor CurrentMoveExecutor => _currentMoveExecutor;
        readonly IMoveExecutorLeaf _regularMoveLeaf;
        readonly IMoveExecutor _slowMoveExecutor;
        readonly IMoveExecutor _confusionMoveExecutor;
        readonly IMoveExecutor _dashMoveExecutor;
        readonly IMoveExecutor _confusionDashMoveExecutor;
        readonly IMoveExecutor _faintedMoveExecutor;
        
        IMoveExecutor _beforeMoveExecutor;
        IMoveExecutor _currentMoveExecutor;

        IMoveExecutor? _beforeMoveExecutorNew;
        
        PlayerInfo _info = null!;
        
        public MoveExecutorSwitcher(
            )
        {
            var regularMoveExecutor = new RegularMoveExecutor(40, 5, 5);
            _regularMoveLeaf = regularMoveExecutor;
            _faintedMoveExecutor = new FaintedMoveDecorator(regularMoveExecutor);
            _confusionMoveExecutor = new InverseInputDecorator(regularMoveExecutor);
            _dashMoveExecutor = new DashMoveDecorator( regularMoveExecutor);
          //   _confusionDashMoveExecutor = new InverseInputDecorator(_dashMoveExecutor);
            _slowMoveExecutor = new SlowMoveDecorator(regularMoveExecutor);
            
            _currentMoveExecutor = _regularMoveLeaf;
            _beforeMoveExecutor = _regularMoveLeaf;
        }

        public void Setup(PlayerInfo info)
        {
            _regularMoveLeaf.Setup(info);
            _slowMoveExecutor.Setup(info);
            _dashMoveExecutor.Setup(info);
            _faintedMoveExecutor.Setup(info);
            _info = info;
        }
        
        public void Move(Vector3 input)
        {
            _currentMoveExecutor.Move(input);
        }
        
        public void SwitchToBeforeMoveExecutor()
        {
            StoreBeforeMove();
            // _currentMoveExecutor =  _beforeMoveExecutor;
            if(_beforeMoveExecutorNew != null) _currentMoveExecutor = _beforeMoveExecutorNew;
        }
        
        public void SwitchToRegularMove()
        {
            StoreBeforeMove();
            _currentMoveExecutor =  _regularMoveLeaf;
        }
        
        public void SwitchToDashMove()
        {
            // StoreBeforeMove();
            // switch (_currentMoveExecutor)
            // {
            //     case InverseInputDecorator _:
            //         _currentMoveExecutor = _confusionDashMoveExecutor;
            //         Debug.Log($"_confusionDashMoveExecutor");
            //         break;
            //     default:
            //         _currentMoveExecutor = _dashMoveExecutor;
            //         break;
            // }
            _beforeMoveExecutorNew = _currentMoveExecutor;
            Debug.Log($"_currentMoveExecutor.GetType() : {_currentMoveExecutor.GetType()}");
            if (_currentMoveExecutor is IMoveExecutorLeaf moveExecutorLeaf)
            {
                var dash = new DashMoveDecorator(moveExecutorLeaf);
                dash.Setup(_info);
                _currentMoveExecutor = dash;
            }
            
        }
        public void SwitchToSlowMove()
        {
            StoreBeforeMove();
            _currentMoveExecutor =  _slowMoveExecutor;
        }
        public void SwitchToConfusionMove()
        {
            StoreBeforeMove();
            _currentMoveExecutor =  _confusionMoveExecutor;
        }
        public void SwitchToFaintedMove()
        {           
            StoreBeforeMove();
            _currentMoveExecutor =  _faintedMoveExecutor;
        }
        
        void StoreBeforeMove()
        {
            if (_currentMoveExecutor is RegularMoveExecutor ||
                _currentMoveExecutor is SlowMoveExecutor || 
                _currentMoveExecutor is InverseInputDecorator)
            {
                if(_currentMoveExecutor == _confusionDashMoveExecutor) return; // dash move is not stored
                _beforeMoveExecutor = _currentMoveExecutor;
            }
        }
                
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _regularMoveLeaf.SetPlayerAnimatorPresenter(presenter);
            _slowMoveExecutor.SetPlayerAnimatorPresenter(presenter);
            _dashMoveExecutor.SetPlayerAnimatorPresenter(presenter);
            _faintedMoveExecutor.SetPlayerAnimatorPresenter(presenter);
        }
    }
}