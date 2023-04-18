using UnityEngine;

namespace Animations
{
    public struct TriggerEnemyAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
    public class TriggerEnemyAnimatorSetter: IAnimatorSimpleEnemyUnit
    {
        private readonly TriggerEnemyAnimatorSetterInfo _info;
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");

        private Animator Animator => _info.Animator;

        public TriggerEnemyAnimatorSetter(TriggerEnemyAnimatorSetterInfo info)
        {
            _info = info;
        }

        public void OnAttack(bool value = true)
        {
            Animator.ResetTrigger(AttackTrigger);
            Animator.SetTrigger(AttackTrigger);
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