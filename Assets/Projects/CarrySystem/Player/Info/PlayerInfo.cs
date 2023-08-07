using System;
using Carry.CarrySystem.Player.Interfaces;
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
        [NonSerialized] public AbstractNetworkPlayerController playerController;

        public void Init(NetworkRunner runner, GameObject playerObj, AbstractNetworkPlayerController playerController)
        {
            this.runner = runner;
            this.playerObj = playerObj;
            this.playerController = playerController;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
    }
}