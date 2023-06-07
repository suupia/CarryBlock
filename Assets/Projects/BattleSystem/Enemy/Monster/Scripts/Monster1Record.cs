using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

# nullable enable

namespace Nuts.BattleSystem.Boss.Scripts
{
    [Serializable]
    public record Monster1Record
    {
        // constant fields
        public readonly float JumpTime = 2f;
        public readonly float ChargeJumpTime = 0.5f;
        public readonly float JumpAttackRadius = 3f;
        public readonly float ChargeSpitOutTime = 1.5f;
        public readonly float SearchRadius = 6f;
        public readonly float DefaultAttackCoolTime = 4f;

        [SerializeField] public Transform? finSpawnerTransform;

        // target buffer
        public HashSet<Transform> TargetBuffer { get; set; } = new();

        // componets
        public GameObject GameObject { get; private set; } // NetworkControllerのGameObject
        public Transform Transform => GameObject.transform;
        public Rigidbody Rb { get; private set; }
        

        // This record will initialize by SerializeField
#pragma warning disable CS8618
        Monster1Record()
#pragma warning restore CS8618
        {
        }

        public void Init(GameObject gameObject)
        {
            GameObject = gameObject;
            Rb = gameObject.GetComponent<Rigidbody>();
        }
    }
}