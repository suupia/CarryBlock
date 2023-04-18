using UnityEngine;

namespace Animations
{
    public struct LoopEnemyAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
    
    public class LoopEnemyAnimatorSetter: IAnimatorSimpleEnemyUnit
    {
        private readonly LoopEnemyAnimatorSetterInfo _info;
        private static readonly int AttackLoop = Animator.StringToHash("AttackLoop");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private Animator Animator => _info.Animator;

        public LoopEnemyAnimatorSetter(LoopEnemyAnimatorSetterInfo info)
        {
            _info = info;
        }
        public void OnAttack(bool value = true)
        {
            Animator.SetBool(AttackLoop, value);
        }

        public void OnDead()
        {
            Animator.SetTrigger(Dead);
        }

        public void OnMove(Vector3 direction)
        {
            if(direction == Vector3.zero) return;
            Animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnSpawn()
        {
        }
    }
}