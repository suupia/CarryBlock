using System;
using Fusion;
using Nuts.Utility.Scripts;
using Nuts.BattleSystem.Move.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Player.Scripts
{
    public interface IUnit : IMove, IUnitAction
    {
    }

    [Serializable]
    public class PlayerInfo
    {
        // Attach
        [SerializeField] public NetworkPrefabRef pickerPrefab;
        [SerializeField] public NetworkPrefabRef bulletPrefab;

        // Property
        public GameObject playerObj;
        public Rigidbody playerRd;

        // constant fields 
        public readonly float acceleration = 30f;
        public readonly float bulletOffset = 1;
        public readonly float maxAngularVelocity = 100f;
        public readonly float maxVelocity = 9f;
        public readonly float rangeRadius = 12.0f;
        public readonly float targetRotationTime = 0.2f;
        [NonSerialized] public NetworkRunner _runner;

        public PlayerInfoForPicker playerInfoForPicker;

        public void Init(NetworkRunner runner, GameObject playerObj)
        {
            _runner = runner;
            this.playerObj = playerObj;
            playerRd = playerObj.GetComponent<Rigidbody>();
            playerInfoForPicker = new PlayerInfoForPicker(this);
        }
    }


    public class PlayerInfoForPicker
    {
        PlayerInfo _info;

        public PlayerInfoForPicker(PlayerInfo info)
        {
            _info = info;
        }

        public float RangeRadius => 12.0f; //ToDo : move to NetworkPlayerInfo
    }
}