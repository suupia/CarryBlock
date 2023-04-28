using Decoration;
using UnityEngine;

namespace Animations
{
    public class TriggerEnemyAnimatorSetter: IDecorationEnemy
    {
        private readonly GameObject _gameObject;
        private readonly Animator _animator;
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");


        public TriggerEnemyAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void OnAttack(bool value = true)
        {
            if (!value) return; 
            _animator.ResetTrigger(AttackTrigger);
            _animator.SetTrigger(AttackTrigger);
        }

        public void OnDamage()
        {
            
        }

        public void OnDead()
        {
            _animator.SetTrigger(Dead);
        }

        public void OnMove()
        {
            // _animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnSpawn()
        {
        }
    }
}