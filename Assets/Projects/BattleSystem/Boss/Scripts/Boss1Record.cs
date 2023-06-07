using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Nuts.BattleSystem.Boss.Scripts
{
    [Serializable]
    public record Boss1Record
    {
        // constant fields
        public readonly float JumpTime = 2f;
        public readonly float ChargeJumpTime = 0.5f;
        public readonly float JumpAttackRadius = 3f;
        public readonly float ChargeSpitOutTime = 1.5f;
        public readonly float SearchRadius = 6f;
        public readonly float DefaultAttackCoolTime = 4f;

        [SerializeField] public Transform finSpawnerTransform;

        // target buffer
        public HashSet<Transform> TargetBuffer { get; set; } = new();

        // componets
        public GameObject GameObject { get; private set; } // NetworkControllerのGameObject
        public Transform Transform => GameObject.transform;
        public Rigidbody Rb { get; private set; }

        NetworkRunner _runner;

        // This record will initialize by SerializeField
        Boss1Record()
        {
        }

        public void Init(NetworkRunner runner, GameObject gameObject)
        {
            _runner = runner;
            GameObject = gameObject;
            Rb = gameObject.GetComponent<Rigidbody>();
        }
    }
}