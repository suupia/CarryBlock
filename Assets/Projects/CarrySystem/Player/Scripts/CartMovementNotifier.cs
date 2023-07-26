﻿using Fusion;
using TMPro;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CartMovementNotifier : NetworkBehaviour
    {
        [SerializeField] TextMeshProUGUI ReachableText = null!;
        [SerializeField] TextMeshProUGUI MoveToCartText = null!;
        
        public void ShowReachableText()
        {
            ReachableText.gameObject.SetActive(true);
        }
        public void ShowMoveToCartText()
        {
            MoveToCartText.gameObject.SetActive(true);
        }
        
        public void HideReachableText()
        {
            ReachableText.gameObject.SetActive(false);
        }
        
        public void HideMoveToCartText()
        {
            MoveToCartText.gameObject.SetActive(false);
        }
    }
}