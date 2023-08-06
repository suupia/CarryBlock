using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace Carry.CarrySystem.Player.Info
{
    [Serializable]
    public record PlayerInfo
    {
        [NonSerialized] public NetworkRunner runner;
        
        // Property
        [NonSerialized] public GameObject playerObj;
        [NonSerialized] public Rigidbody playerRb;

        public void Init(NetworkRunner runner, GameObject playerObj)
        {
            this.runner = runner;
            this.playerObj = playerObj;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
    }
}