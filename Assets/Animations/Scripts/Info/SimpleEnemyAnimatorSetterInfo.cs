using System;
using UnityEngine;

namespace Network.AnimatorSetter.Info
{        
    [Serializable]
    public struct SimpleEnemyAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
}