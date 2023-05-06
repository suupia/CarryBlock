using Decoration;
using UnityEngine;

namespace Animations
{
    public class TankAnimatorSetter : IPlayerDecoration
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private readonly Animator _animator;
        private readonly GameObject _gameObject;

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