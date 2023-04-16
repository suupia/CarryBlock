using UnityEngine;

namespace Animations
{
    public struct SimpleEnemyAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
    public class SimpleEnemyAnimatorSetter: IAnimatorSimpleEnemyUnit
    {
        private readonly SimpleEnemyAnimatorSetterInfo _info;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public SimpleEnemyAnimatorSetter(SimpleEnemyAnimatorSetterInfo info)
        {
            _info = info;
        }

        public void OnAttack()
        {
            _info.Animator.SetTrigger(Attack);
        }

        public void OnDead()
        {
            _info.Animator.SetTrigger(Dead);
        }

        public void OnMove(Vector3 direction)
        {
            if(direction == Vector3.zero) return;
            _info.Animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnSpawn()
        {
        }
    }
}