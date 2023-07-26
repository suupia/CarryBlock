using System;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class PlayerNearCartHandlerNet : NetworkBehaviour
    {
        HoldingBlockObserver _holdingBlockObserver =null!;

        [Inject]
        public void Construct(HoldingBlockObserver holdingBlockObserver)
        {
            _holdingBlockObserver = holdingBlockObserver;
        }
        
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

    }
}