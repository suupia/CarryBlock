using UnityEngine;

namespace Animations
{
    public struct LoopEnemyAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
    
    public class LoopEnemyAnimatorSetter
    {
        private LoopEnemyAnimatorSetterInfo _info;

        private Animator Animator => _info.Animator;

        public LoopEnemyAnimatorSetter(LoopEnemyAnimatorSetterInfo info)
        {
            _info = info;
        }
        
    }
}