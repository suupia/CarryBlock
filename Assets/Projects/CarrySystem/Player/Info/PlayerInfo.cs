using System;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Carry.CarrySystem.Player.Info
{
    [Serializable]
    public record PlayerInfo
    {
        // Property
        [NonSerialized] public GameObject PlayerObj = null!;
        [NonSerialized] public Rigidbody PlayerRb = null!;
        [NonSerialized] public AbstractNetworkPlayerController PlayerController = null!;
        [NonSerialized] public PlayerRef PlayerRef;

        public void Init(GameObject playerObj, AbstractNetworkPlayerController playerController,PlayerRef playerRef)
        {
            this.PlayerObj = playerObj;
            this.PlayerController = playerController;
            this.PlayerRef = playerRef;
            PlayerRb = playerObj.GetComponent<Rigidbody>();
        }
    }
}