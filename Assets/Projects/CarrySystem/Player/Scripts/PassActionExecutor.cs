using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Carry.CarrySystem.Block.Interfaces;
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
        readonly HoldActionExecutor _holdActionExecutor;
        readonly float _radius;
        readonly int _layerMask;
        readonly  Collider[] _targetBuffer = new Collider[10];
        readonly PlayerHoldingObjectContainer _holdingObjectContainer;
        readonly IPlayerBlockPresenter _playerPresenterContainer;

        PassRangeNet? _passRangeNet;

        public PassActionExecutor(
            PlayerHoldingObjectContainer holdingObjectContainer,
            IPlayerBlockPresenter playerPresenterContainer,
            HoldActionExecutor holdActionExecutor,
            float radius,
            int layerMask)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _playerPresenterContainer = playerPresenterContainer;
            _holdActionExecutor = holdActionExecutor;
            _radius = radius;
            _layerMask = layerMask; /*LayerMask.GetMask("Player");*/
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
                if(!canPass.Item1) return;
                var block = canPass.Item2;
                PassBlock(block);
                targetPlayerController.GetCharacter.ReceivePass(block);
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
            _playerPresenterContainer.ReceiveBlock(block);
        }
        
        // private
        void PassBlock(ICarriableBlock block)
        {
            Debug.Log($"Pass Block");
            block.PutDown(_info.PlayerController.GetCharacter);
            _playerPresenterContainer.PassBlock();
        }
        
        (bool, ICarriableBlock) CanPass(CarryPlayerControllerNet targetPlayerController)
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
        
        
        
    }
}