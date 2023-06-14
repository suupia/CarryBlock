using System;
using Fusion;
using Nuts.Utility.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Carry.CarrySystem.Player.Scripts
{
    [Serializable]
    public class PlayerInfo
    {
        [NonSerialized] public NetworkRunner runner;
        
        // Property
        [NonSerialized] public GameObject playerObj;
        [NonSerialized] public Rigidbody playerRb;

        // constant fields 
        public readonly float acceleration = 30f;
        public readonly float maxVelocity = 9f;
        public readonly float maxAngularVelocity = 100f;
        public readonly float targetRotationTime = 0.2f;

        public void Init(NetworkRunner runner, GameObject playerObj)
        {
            this.runner = runner;
            this.playerObj = playerObj;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
    }
}