using System;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using System.Threading;
using Carry.CarrySystem.Map.Interfaces;
using Cysharp.Threading.Tasks;
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
        IMapUpdater _mapUpdater;

        [Inject]
        public PlayerFollowMovingCart(
            CarryPlayerContainer carryPlayerContainer,
            IMapUpdater mapUpdater
        )
        {
            _carryPlayerContainer = carryPlayerContainer;
            _mapUpdater = mapUpdater;
        }

        public void FollowMovingCart()
        {
            int _mapNumberSaver = _mapUpdater.Index;
            
            _cancellationDisposable = Observable.EveryUpdate()
                .Subscribe(_ =>
                {
                    CartControllerNet cart = FindObjectOfType<CartControllerNet>();
                    
                    foreach (CarryPlayerControllerNet player in _carryPlayerContainer.PlayerControllers)
                    {
                        Transform playerTransform = player.transform;
                        Transform cartTransform = cart.transform;

                        Vector3 newPosition = cartTransform.position + Vector3.up; 
                        
                        playerTransform.position = newPosition;

                        //_mapNumberSaverはカートに乗った瞬間の_mapUpdater.Indexが代入される
                        //_mapUpdater.Indexはマップが切り替わったら1増える
                        //マップが切り替わったらプレイヤーはカートから解放される
                        if (_mapNumberSaver != _mapUpdater.Index) 
                        {
                            CancelFollowMovingCart();
                        }
                    }
                    
                });
            
        }

        public void CancelFollowMovingCart()
        {
            _cancellationDisposable?.Dispose();
        }
        
       
    }
    
}