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
        readonly IMoveExecutorLeaf _confusionMoveLeaf;
        readonly IMoveExecutor _dashMoveExecutor;
        readonly IMoveExecutor _faintedMoveExecutor;
        
        IMoveExecutor _currentMoveExecutor;

        IMoveExecutor? _beforeMoveExecutorNew;
        
        PlayerInfo _info = null!;
        
        public MoveExecutorSwitcher(
            )
        {
            var regularMoveExecutor = new RegularMoveExecutor(40, 5, 5);
            _regularMoveLeaf = regularMoveExecutor;
            _confusionMoveLeaf = new InverseInputMoveExecutorLeaf(regularMoveExecutor);
            _faintedMoveExecutor = new FaintedMoveDecorator(regularMoveExecutor);
            _dashMoveExecutor = new DashMoveDecorator( regularMoveExecutor);
            _slowMoveExecutor = new SlowMoveDecorator(regularMoveExecutor);
            
            _currentMoveExecutor = _regularMoveLeaf;
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
            Debug.Log($"Move _currentMoveExecutor.GetType() : {_currentMoveExecutor.GetType()}");
            if(_currentMoveExecutor is IMoveExecutorLeaf leaf) Debug.Log($"Move _currentMoveExecutor.MaxVelocity : {leaf.MaxVelocity}");
        }
        
        public void SwitchToBeforeMoveExecutor()
        {
            if(_beforeMoveExecutorNew != null) _currentMoveExecutor = _beforeMoveExecutorNew;
        }
        
        public void SwitchToRegularMove()
        {
            _currentMoveExecutor =  _regularMoveLeaf;
        }
        
        public void SwitchToDashMove()
        {

            _beforeMoveExecutorNew = _currentMoveExecutor;
            if (_currentMoveExecutor is IMoveExecutorLeaf moveExecutorLeaf)
            {
                var dash = new DashMoveDecorator(moveExecutorLeaf);
                dash.Setup(_info);
                _currentMoveExecutor = dash;
            }
            
        }
        public void SwitchToSlowMove()
        {
            _currentMoveExecutor =  _slowMoveExecutor;
        }
        public void SwitchToConfusionMove()
        {
            _currentMoveExecutor =  _confusionMoveLeaf;
        }
        public void SwitchToFaintedMove()
        {           
            _currentMoveExecutor =  _faintedMoveExecutor;
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