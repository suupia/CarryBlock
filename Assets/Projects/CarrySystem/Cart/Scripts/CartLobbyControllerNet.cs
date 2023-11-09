using System;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartLobbyControllerNet : NetworkBehaviour
    {
        public Collider DetectCollider => detectCollider;
        
        [SerializeField] Collider detectCollider = null!;
        
        CartShortestRouteMove _move = null!; 
        
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