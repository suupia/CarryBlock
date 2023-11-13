using System;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
#nullable enable

namespace Carry.CarrySystem.Player.Info
{
    public record PlayerInfo
    {
        // Property
        public GameObject PlayerObj = null!;
        public Rigidbody PlayerRb = null!;
        public AbstractNetworkPlayerController PlayerController = null!;
        public PlayerRef PlayerRef;

        public void Init(AbstractNetworkPlayerController playerController,PlayerRef playerRef)
        {
            PlayerObj = playerController.gameObject;
            PlayerController = playerController;
            PlayerRef = playerRef;
            PlayerRb = playerController.GetComponent<Rigidbody>();
        }
    }
}