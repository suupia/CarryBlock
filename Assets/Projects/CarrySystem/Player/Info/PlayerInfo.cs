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
        [NonSerialized] public PlayerRef playerRef;

        public void Init(NetworkRunner runner, GameObject playerObj, AbstractNetworkPlayerController playerController,PlayerRef playerRef   )
        {
            this.runner = runner;
            this.playerObj = playerObj;
            this.playerController = playerController;
            this.playerRef = playerRef;
            playerRb = playerObj.GetComponent<Rigidbody>();
        }
    }
}