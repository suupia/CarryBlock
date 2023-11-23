using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        readonly IMoveExecutorLeaf _confusionMoveLeaf;
        
        IMoveExecutor _currentMoveExecutor;
        IMoveExecutorLeaf _beforeMoveExecutorLeaf;
        
        PlayerInfo _info = null!;
        IPlayerAnimatorPresenter _playerAnimatorPresenter = null!;
        
        public MoveExecutorSwitcher(
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
            _currentMoveExecutor.Move(input);
            
            // Debug.Log($"Move _currentMoveExecutor.GetType() : {_currentMoveExecutor.GetType()}");
            // if(_currentMoveExecutor is IMoveExecutorLeaf leaf) Debug.Log($"Move _currentMoveExecutor.MaxVelocity : {leaf.MaxVelocity}");
            //
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
    
    
    ///////////////////////////////////////////////////

    public interface IMoveFunction : IMoveExecutor
    {
        IMoveFunction Chain(IMoveFunction moveExecutor);
    }
    public class NewFastMoveExecutor : IMoveExecutor, IMoveFunction
    {
        public void Setup(PlayerInfo info)
        {
            throw new System.NotImplementedException();
        }
        
        public IMoveFunction Chain(IMoveFunction moveExecutor)
        {
            throw new System.NotImplementedException();
        }

        public void Move(Vector3 input)
        {
            throw new System.NotImplementedException();
        }

        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            throw new System.NotImplementedException();
        }
    }
        
    public class NewMoveSwitcher
    {
        IList<IMoveFunction> _moveExecutors = new List<IMoveFunction>();

        public void Move(Vector3 input)
        {
            IMoveFunction move = _moveExecutors.Aggregate((IMoveFunction)new NewFastMoveExecutor(), (integrated, next) =>  next.Chain(integrated));
            move.Move(input);
            
        }
        
    }

    public class Client
    {
        void Start()
        {
            var moveExecutorSwitcher = new MoveExecutorSwitcher();
            moveExecutorSwitcher.Move(Vector3.up);
        }
    }
}