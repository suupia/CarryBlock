using Decoration;
using UnityEngine;

namespace Animations
{
    public class LoopEnemyAnimatorSetter: IDecorationEnemy
    {
        private readonly GameObject _gameObject;
        private readonly Animator _animator;
        private static readonly int AttackLoop = Animator.StringToHash("AttackLoop");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");

        public LoopEnemyAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }
        public void OnAttack(bool value = true)
        {
            _animator.SetBool(AttackLoop, value);
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