using System;
using UnityEngine;

namespace Network.AnimatorSetter.Info
{
    [Serializable]

    public struct TankAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
        public GameObject ShooterObject { get; set; }
    }
}