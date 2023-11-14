using Projects.CarrySystem.Cart.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Cart.Scripts
{
    public class HoldingBlockNotifierLogger : IHoldingBlockNotifier
    {
        public void ShowReachableText()
        {
            Debug.Log($"ShowReachableText()");
        }

        public void ShowMoveToCartText()
        {
            Debug.Log($"ShowMoveToCartText()");
        }

        public void HideReachableText()
        {
            Debug.Log($"HideReachableText()");
        }

        public void HideMoveToCartText()
        {
            Debug.Log($"HideMoveToCartText()");
        }
    }
}