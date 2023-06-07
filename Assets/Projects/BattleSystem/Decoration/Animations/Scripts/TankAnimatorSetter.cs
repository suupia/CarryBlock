using Nuts.Projects.BattleSystem.Decoration.Scripts;
using UnityEngine;

namespace Nuts.BattleSystem.Decoration.Scripts
{
    public class TankAnimatorSetter : IPlayerDecoration
    {
        static readonly int Speed = Animator.StringToHash("Speed");
        readonly Animator _animator;
        readonly GameObject _gameObject;

        public TankAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void OnMoved()
        {
            // _animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnDamaged()
        {
        }

        public void OnAttacked(bool onStart = true)
        {
        }

        public void OnMainAction()
        {
        }

        public void OnDead()
        {
        }

        public void OnSpawned()
        {
        }

        public void OnChangeDirection(Vector3 direction)
        {
        }
    }
}