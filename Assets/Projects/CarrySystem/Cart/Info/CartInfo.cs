using System;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Cart.Info
{
    public record CartInfo
    {
        // Property
        [NonSerialized] public GameObject cartObj;
        [NonSerialized] public Rigidbody cartRb;
        

        public  CartInfo(GameObject cartObj)
        {
            this.cartObj = cartObj;
            this.cartRb = cartObj.GetComponent<Rigidbody>();
        }
    }
}