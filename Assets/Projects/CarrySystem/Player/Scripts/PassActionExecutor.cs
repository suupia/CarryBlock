using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.CarriableBlock.Interfaces;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PassActionExecutor : IPassActionExecutor
    {
        PlayerInfo _info = null!;
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;
        readonly PassWaitExecutor _passWaitExecutor;
        
        // Presenter
        IPlayerBlockPresenter? _playerBlockPresenter;
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        PassRangeNet? _passRangeNet;
        PassBlockMoveExecutor? _passBlockMoveExecutor;

        public PassActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer,
            PassWaitExecutor passWaitExecutor)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _passWaitExecutor = passWaitExecutor;

        }
        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Reset()
        {
            
        }

        public void PassAction()
        {
            if (_passRangeNet == null) _passRangeNet = _info.PlayerController.GetComponentInChildren<PassRangeNet>();
            if (_passRangeNet.DetectedTarget() is {} target)
            {
                var targetPlayerController  = target.GetComponent<CarryPlayerControllerNet>();
                if (targetPlayerController == null)
                {
                    Debug.LogError($"{target.name} には CarryPlayerControllerNet がアタッチされていません");
                    return;
                }
                
                Debug.Log($"{_info.PlayerController.Object.InputAuthority}から{targetPlayerController.Object.InputAuthority}に対してPassを試みます");
                
                var canPass = CanPass(targetPlayerController);
                if(!canPass.CanPass) return;
                var block = canPass.CarriableBlock;
                PassBlock(block);
                _passWaitExecutor.WaitPassAction(targetPlayerController.GetCharacter, block);
                _passBlockMoveExecutor?.PassBlockMove(block, targetPlayerController.GetCharacter);
            }
        }

        // public
        public bool CanReceivePass()
        {
            return !_holdingObjectContainer.IsHoldingBlock;
        }
        public void ReceivePass(ICarriableBlock block)
        {
            Debug.Log("Receive Pass");
            block.PickUp(_info.PlayerController.GetCharacter);
            _holdingObjectContainer.SetBlock(block);
            _playerBlockPresenter?.ReceiveBlock(block);
            _playerAnimatorPresenter?.ReceiveBlock(block);
        }
        
        // private
        void PassBlock(ICarriableBlock block)
        {
            Debug.Log($"Pass Block");
            block.PutDown(_info.PlayerController.GetCharacter);
            _playerBlockPresenter?.PassBlock();
            _playerAnimatorPresenter?.PassBlock();
        }
        
        (bool CanPass, ICarriableBlock CarriableBlock) CanPass(CarryPlayerControllerNet targetPlayerController)
        {
            if (!_holdingObjectContainer.IsHoldingBlock) 
            {
                Debug.Log($"{_info.PlayerController.Object.InputAuthority} isn't holding a block. So, can't pass block");
                return (false, null!);
            }
            if (!targetPlayerController.GetCharacter.CanReceivePass())
            {
                Debug.Log($"{targetPlayerController.Object.InputAuthority} is holding a block.So, can't receive pass");
                return (false, null!);
            }
            var block = _holdingObjectContainer.PopBlock();  // 判定の一番最後にPopする
            if (block == null)
            {
                Debug.LogError($"block is null");  // IsHoldingBlockがtrueなのに、blockがnullなのはおかしい
                return (false, null!);
            }
            return (true, block);
        }
        
        // Presenter
        public void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter)
        {
            _playerBlockPresenter = presenter;
        }
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
        
        public void SetPassBlockMoveExecutor(PassBlockMoveExecutor passBlockMoveExecutor)
        {
            _passBlockMoveExecutor = passBlockMoveExecutor;
        }
        
    }
}