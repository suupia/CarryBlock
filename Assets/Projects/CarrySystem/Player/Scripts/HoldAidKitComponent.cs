using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class HoldAidKitComponent : IHoldableComponent
    {
        PlayerInfo _info = null!;

        readonly PlayerHoldingObjectContainer _holdingObjectContainer;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        
        IPlayerHoldablePresenter? _playerAidKitPresenter;
        AidKitRangeNet? _aidKitRangeNet;
        
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;  // 今は使用しない。なぜなら、AidKit取得に関係するアニメーションがないから


        public HoldAidKitComponent(
            PlayerHoldingObjectContainer holdingObjectContainer,
            PlayerNearCartHandlerNet playerNearCartHandler)
        {
            _holdingObjectContainer = holdingObjectContainer;
            _playerNearCartHandler = playerNearCartHandler;
        }
        
        public void Setup(PlayerInfo info)
        {
            _info = info;
        }
        
        public void ResetHoldable()
        {
            _holdingObjectContainer.PopAidKit();
            if (_playerAidKitPresenter != null) _playerAidKitPresenter.DisableHoldableView();
        }
        public bool TryToPickUpHoldable()
        {
            // もしカートの近くにいれば、AidKitを拾う
            if (_playerNearCartHandler.IsNearCart(_info.PlayerObj))
            {
                if(_holdingObjectContainer.IsHoldingAidKit) return false;
                
                // 拾う処理
                Debug.Log($"PickUpAidKit");
                _holdingObjectContainer.SetAidKit();
                if(_playerAidKitPresenter != null) _playerAidKitPresenter.EnableHoldableView(new EmptyHoldable());
            }
            return false;
        }

        public bool TryToUseHoldable()
        {
            // もし倒れているキャラが近くにいれば、AidKitを使う
            // 1. PlayerControllerを取得
            // 2. ICharacterを取得
            // 3. IsFaintedで判定
                
            if(_aidKitRangeNet == null) _aidKitRangeNet = _info.PlayerObj.GetComponentInChildren<AidKitRangeNet>();
                
            if(_aidKitRangeNet.DetectedTarget() is {} target)
            {
                var targetPlayerController = target.GetComponent<CarryPlayerControllerNet>();
                if (targetPlayerController == null)
                {
                    Debug.LogError($"{target.name} には CarryPlayerControllerNet がアタッチされていません");
                    return false;
                }
                if (!targetPlayerController.GetOnDamageExecutor.IsFainted) return false;
                Debug.Log($"Use AidKit");
                _holdingObjectContainer.PopAidKit();
                if(_playerAidKitPresenter != null) _playerAidKitPresenter.DisableHoldableView();
                targetPlayerController.GetOnDamageExecutor.OnRevive();
            }
            else
            {
                // Do nothing
            }

            return true;
        }
        
        // View
        public void SetPlayerHoldablePresenter(IPlayerHoldablePresenter presenter)
        {
            _playerAidKitPresenter = presenter;
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}