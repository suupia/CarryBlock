using System;
using Fusion;
using Projects.CarrySystem.Item.Scripts;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Cart.Scripts
{
    public class CartControllerNet : NetworkBehaviour
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

        void OnTriggerEnter(Collider other)
        {
            
            if (other.CompareTag("Item"))
            {
                var itemView= other.gameObject;
                var itemController = itemView.transform.parent.GetComponent<ItemControllerNet>(); // ItemControllerNet was attached to parent object
                if (itemController == null)
                {
                    Debug.LogWarning($"itemController is null");
                    return;
                }
                itemController.OnGain();
            }
            
        }
    }
}