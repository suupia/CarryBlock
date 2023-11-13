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
        public GameObject PlayerObj { get; private set; }  // This is controller's gameObject and not interpolated gameObject by NetworkRigidbody
        public Rigidbody PlayerRb { get; private set; }
        public IPlayerController PlayerController { get; private set; } 
        public PlayerRef PlayerRef { get; private set; }

        public PlayerInfo(IPlayerController playerController,PlayerRef playerRef)
        {
            PlayerObj = playerController.GameObject;
            PlayerController = playerController;
            PlayerRef = playerRef;
            PlayerRb = playerController.Rigidbody;
        }    

    }
}