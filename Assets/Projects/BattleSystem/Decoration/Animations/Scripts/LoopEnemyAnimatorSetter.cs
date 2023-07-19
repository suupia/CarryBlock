using Projects.Projects.BattleSystem.Decoration.Scripts;
using UnityEngine;

namespace Projects.BattleSystem.Decoration.Scripts
{
    public class LoopEnemyAnimatorSetter : IEnemyDecoration
    {
        static readonly int AttackLoop = Animator.StringToHash("AttackLoop");
        static readonly int Dead = Animator.StringToHash("Dead");
        static readonly int Speed = Animator.StringToHash("Speed");
        readonly Animator _animator;
        readonly GameObject _gameObject;

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