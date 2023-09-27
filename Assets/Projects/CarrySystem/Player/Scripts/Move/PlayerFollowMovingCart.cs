using System;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerFollowMovingCart : NetworkBehaviour
    {
        readonly CarryPlayerContainer _carryPlayerContainer;
        CancellationTokenSource? _cancellationTokenSource;

        [Inject]
        public PlayerFollowMovingCart(
            CarryPlayerContainer carryPlayerContainer
        )
        {
            _carryPlayerContainer = carryPlayerContainer;
        }

        public void FollowMovingCart()
        {
            // CancellationToken ct = this.GetCancellationTokenOnDestroy();
            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            DelayAsync(token).Forget();
        }

        private async UniTask DelayAsync(CancellationToken token)
        {
            try
            {
                CartControllerNet cart = FindObjectOfType<CartControllerNet>();

                foreach (CarryPlayerControllerNet player in _carryPlayerContainer.PlayerControllers)
                {
                    Transform playerTransform = player.transform;
                    Transform cartTransform = cart.transform;

                    playerTransform.position = cartTransform.position;
                }
                
                await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: token);

            }catch (OperationCanceledException)
            {
                // Do nothing
            }

        }
        //UnityEngine.Object.
    }
}