using Fusion;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartControllerNet : NetworkBehaviour
    {
        public Collider DetectCollider => detectCollider;
        
        [SerializeField] Collider detectCollider = null!;
        
        CartShortestRouteMove _move; 
        
        public void Init(CartShortestRouteMove move)
        {
            _move = move;
        }

        public void StartMove()
        {
            _move.MoveAlongWithShortestRoute();
        }
        
    }
}