using System;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.GimmickBlock.Scripts
{
    public class CannonBallInfo
    {
        // Property
        [NonSerialized] public GameObject CannonBallObj;
        [NonSerialized] public Rigidbody CannonBallRb;
        

        public  CannonBallInfo(GameObject cartObj)
        {
            this.CannonBallObj = cartObj;
            this.CannonBallRb = cartObj.GetComponent<Rigidbody>();
        }
    }
}