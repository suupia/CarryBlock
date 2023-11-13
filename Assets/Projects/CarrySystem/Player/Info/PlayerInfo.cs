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
        public GameObject PlayerObj { get; private set; }
        public Rigidbody PlayerRb { get; private set; }
        public AbstractNetworkPlayerController PlayerController { get; private set; } 
        public PlayerRef PlayerRef { get; private set; }

        public PlayerInfo(AbstractNetworkPlayerController playerController,PlayerRef playerRef)
        {
            PlayerObj = playerController.gameObject;
            PlayerController = playerController;
            PlayerRef = playerRef;
            PlayerRb = playerController.GetComponent<Rigidbody>();
        }    

    }
}