using System;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Fusion;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    /// <summary>
    /// PlayerやすべてのPlayerがカートの近くにいるかを判定するクラス
    /// </summary>
    public class PlayerNearCartHandlerNet : NetworkBehaviour
    {
        HoldingBlockObserver _holdingBlockObserver =null!;
        CarryPlayerContainer _carryPlayerContainer = null!;
        IMapUpdater _mapUpdater = null!;
        
        bool _isCartStarted = false;
        
        [Inject]
        public void Construct(
            HoldingBlockObserver holdingBlockObserver,
            CarryPlayerContainer carryPlayerContainer,
            IMapUpdater mapUpdater
            )
        {
            _holdingBlockObserver = holdingBlockObserver;
            _carryPlayerContainer = carryPlayerContainer;
            _mapUpdater = mapUpdater;
            
            
            this.UpdateAsObservable()
                .Where(_ => _holdingBlockObserver.IsMapClear)
                .Where(_ => !_isCartStarted)
                .Subscribe(_ =>
                {
                    // すべてのプレイヤーがカートの近くにいれば、カートを動かす
                    if (IsAllPlayerNearCart())
                    {
                        var cart = FindObjectOfType<CartControllerNet>();
                        cart.StartMove();
                        _isCartStarted = true;
                    }
                });
            
            _mapUpdater.RegisterResetAction(() =>
            {
                _isCartStarted = false;
            });
        }

        public bool IsNearCart(GameObject player)
        {
            var cart = FindObjectOfType<CartControllerNet>();
            var detectCollider = cart.DetectCollider;
            return detectCollider.ClosestPoint(player.transform.position) == player.transform.position;

        }
        
        public bool IsAllPlayerNearCart()
        {
            foreach (var player in _carryPlayerContainer.PlayerControllers)
            {
                if (!IsNearCart(player.gameObject))
                {
                    return false;
                }
            }
            
            return true;
        }

        void FixedUpdate()
        {
            if(! HasStateAuthority) return;
            
        }

        #if UNITY_EDITOR
        void Update()
        {
            if(Runner == null) return;
            
            // コライダーで周囲にいるかを判定し、さらにプレイヤーの入力で判定する
            if(Runner.IsServer && Input.GetKeyDown(KeyCode.F))
            {
                if (_holdingBlockObserver.IsMapClear)
                {
                    // カートを動かす
                    var cart = GameObject.FindObjectOfType<CartControllerNet>();
                    cart.StartMove();
                }

            }
            
            
        }
        #endif

    }
}