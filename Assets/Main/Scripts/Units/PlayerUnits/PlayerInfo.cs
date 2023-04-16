using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main
{
    public interface IPlayerUnit
    {
        void Move(Vector3 direction);
        float ActionCooldown();
        void Action();
    }

    [Serializable]
    public  class PlayerInfo
    {
        [NonSerialized]public NetworkRunner _runner;
    
        // constant fields 
        public readonly float acceleration = 10f;
        public readonly float maxVelocity = 15f;
        public readonly float resistance = 0.9f;
        public readonly float bulletOffset = 1;
        public readonly float rangeRadius = 12.0f;
    
        // Attach
        [SerializeField] public NetworkPrefabRef pickerPrefab;
        [SerializeField] public NetworkPrefabRef bulletPrefab;
    
        // Property
        public GameObject playerObj;
        public Rigidbody playerRd;

        public PlayerInfoForPicker playerInfoForPicker;
        public void Init(NetworkRunner runner, GameObject playerObj)
        {
            _runner = runner;
            this.playerObj = playerObj;
            this.playerRd = playerObj.GetComponent<Rigidbody>();
            playerInfoForPicker = new PlayerInfoForPicker(this);
        }
    }


    public class PlayerInfoForPicker
    {
        public float RangeRadius => 12.0f; //ToDo : move to NetworkPlayerInfo
        PlayerInfo _info;

        public PlayerInfoForPicker(PlayerInfo info)
        {
            _info = info;
        }
    }

}

