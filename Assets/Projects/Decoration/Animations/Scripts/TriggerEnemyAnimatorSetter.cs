using Decoration;
using UnityEngine;

namespace Animations
{
    public class TriggerEnemyAnimatorSetter : IEnemyDecoration
    {
        private static readonly int AttackTrigger = Animator.StringToHash("AttackTrigger");
        private static readonly int Dead = Animator.StringToHash("Dead");
        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly Animator _animator;
        private readonly GameObject _gameObject;


        public TriggerEnemyAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void OnAttacked(bool onStart = true)
        {
            if (!onStart) return;
            _animator.ResetTrigger(AttackTrigger);
            _animator.SetTrigger(AttackTrigger);
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