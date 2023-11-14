using Projects.CarrySystem.Cart.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Cart.Scripts
{
    // todo: クラス名を実装に合わせて適切なものに変更する
    public class HoldingBlockNotifierLogger : MonoBehaviour, IHoldingBlockNotifier
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