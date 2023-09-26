using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Info;
using Fusion;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class PlayerFollowMovingCart : NetworkBehaviour
    {
        CarryPlayerContainer _carryPlayerContainer = null!;

        public void Construct(CarryPlayerContainer carryPlayerContainer)
        {
            _carryPlayerContainer = carryPlayerContainer;
        }
        public void FollowMovingCart()
            {
                CartControllerNet cart = FindObjectOfType<CartControllerNet>(); 
                Debug.Log("Player is following the cart.");
            }
    }
}