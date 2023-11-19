using Fusion;
using Projects.CarrySystem.Cart.Interfaces;
using TMPro;
using UnityEngine;
#nullable enable
namespace Carry.CarrySystem.Cart.Scripts
{
    public class HoldingBlockNotifierNet : NetworkBehaviour, IHoldingBlockNotifier  
    {
        // UIに特化させる
        // このクラスにドメインに関係する処理を足してはいけない。
        // つまり、このクラスを参照してよいのはUIのみである。
        [Networked] public NetworkBool IsReachable { get; set; }
        [Networked] public NetworkBool IsMoveToCart { get; set; }
        [SerializeField] TextMeshProUGUI ReachableText = null!;
        [SerializeField] TextMeshProUGUI MoveToCartText = null!;
        
        public override void Render()
        {
            ReachableText.gameObject.SetActive(IsReachable);
            MoveToCartText.gameObject.SetActive(IsMoveToCart);
        }
        public void ShowReachableText()
        {
            ReachableText.gameObject.SetActive(true);
            IsReachable = true;
        }
        public void ShowMoveToCartText()
        {
            MoveToCartText.gameObject.SetActive(true);
            IsMoveToCart = true;
        }
        
        public void HideReachableText()
        {
            ReachableText.gameObject.SetActive(false);
            IsReachable = false;
        }
        
        public void HideMoveToCartText()
        {
            MoveToCartText.gameObject.SetActive(false);
            IsMoveToCart = false;
        }
    }
}