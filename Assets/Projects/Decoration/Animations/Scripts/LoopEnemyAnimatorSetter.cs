using Decoration;
using UnityEngine;

namespace Animations
{
    public class LoopEnemyAnimatorSetter : IEnemyDecoration
    {
        private static readonly int AttackLoop = Animator.StringToHash("AttackLoop");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly Animator _animator;
        private readonly GameObject _gameObject;

        public LoopEnemyAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void OnAttacked(bool onStart = true)
        {
            _animator.SetBool(AttackLoop, onStart);
        }

        public void OnDamaged()
        {
        }

        public void OnDead()
        {
            _animator.SetTrigger(Dead);
        }

        public void OnMoved()
        {
            // _animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnSpawned()
        {
        }
    }
}