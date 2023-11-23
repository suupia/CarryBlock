using System;
using System.Linq;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using System.Threading;
using Carry.CarrySystem.Map.Interfaces;
using Fusion;
using UnityEngine;
using VContainer;
using UniRx;
using VContainer.Unity;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerFollowMovingCart : NetworkBehaviour
    {
        readonly CarryPlayerContainer _carryPlayerContainer;

        IDisposable? _cancellationDisposable;
        readonly IMapGetter _mapGetter;

        [Inject]
        public PlayerFollowMovingCart(
            CarryPlayerContainer carryPlayerContainer,
            IMapGetter mapGetter
        )
        {
            _carryPlayerContainer = carryPlayerContainer;
            _mapGetter = mapGetter;
        }

        public void FollowMovingCart()
        {
            int mapNumberSaver = _mapGetter.Index;
            
            _cancellationDisposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    CartControllerNet cart = FindObjectOfType<CartControllerNet>();
                    
                    foreach (var player in _carryPlayerContainer.PlayerControllers.Select((value, index) => (value, index)))
                    {
                        if (player.value == null)  // タイトルに戻るときにplayerがおそらく先にDestroyされている
                        {
                            CancelFollowMovingCart();
                            return;
                        }
                        Transform playerTransform = player.value.transform;

                        Vector3 centerPosition =  cart.transform.position + Vector3.up; 
                        
                        playerTransform.position = PlayerPositionCalculator.CalcPlayerPosition(
                            centerPosition,
                            player.index,
                            _carryPlayerContainer.PlayerControllers.Count,
                            radius:2.0f);

                        //_mapNumberSaverはカートに乗った瞬間の_mapUpdater.Indexが代入される
                        //_mapUpdater.Indexはマップが切り替わったら1増える
                        //マップが切り替わったらプレイヤーはカートから解放される
                        if (mapNumberSaver != _mapGetter.Index) 
                        {
                            CancelFollowMovingCart();
                        }
                    }
                    
                });
            
        }

        void CancelFollowMovingCart()
        {
            _cancellationDisposable?.Dispose();
        }
        
        void OnDestroy()
        {
            // 一応書いてあるが、今は呼ばれないはず。　player == nullのときは直接CancelFollowMovingCart()を呼ぶようになっている
            Debug.Log($"OnDestroy PlayerFollowMovingCart");
            CancelFollowMovingCart();
        }
       
    }
    
}