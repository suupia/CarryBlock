using System;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Fusion;
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
        
        // public bool AllPlayerNearCart => 
        
        float _detectRadius = 1f; 
        [Inject]
        public void Construct(HoldingBlockObserver holdingBlockObserver)
        {
            _holdingBlockObserver = holdingBlockObserver;
            
            // ToDo: プレイヤーコンテナを受け取る
        }

        public bool IsNearCart(GameObject player)
        {
            var cart = GameObject.FindObjectOfType<CartControllerNet>();
            var detectCollider = cart.DetectCollider;
            return detectCollider.ClosestPoint(player.transform.position) == player.transform.position;
        }
        
        public bool IsAllPlayerNearCart()
        {
            // var players = GameObject.FindObjectsOfType<CarryPlayerControllerNet>();
            // foreach (var player in players)
            // {
            //     if (!IsNearCart(player.gameObject))
            //     {
            //         return false;
            //     }
            // }
            //
            // return true;
            return false;
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