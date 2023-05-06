using Decoration;
using UnityEngine;

namespace Animations
{
    public class PlaneAnimatorSetter : IPlayerDecoration
    {
        private static readonly int BodyState = Animator.StringToHash("BodyState");
        private readonly Animator _animator;

        private readonly GameObject _gameObject;
        private Vector3 _preForward = Vector3.zero;

        public PlaneAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }


        public void OnAttacked(bool onStart = true)
        {
        }

        public void OnDamaged()
        {
        }

        public void OnDead()
        {
        }

        public void OnMoved()
        {
        }

        public void OnSpawned()
        {
        }

        public void OnMainAction()
        {
        }

        public void OnChangeDirection(Vector3 direction)
        {
            // Debug.Log(direction);
            var deltaAngle = direction.x;
            var bodyState = deltaAngle switch
            {
                < 0 => BodyStates.Left,
                > 0 => BodyStates.Right,
                _ => BodyStates.Normal
            };

            _animator.SetInteger(BodyState, (int)bodyState);
        }

        private enum BodyStates
        {
            Normal = 0,
            Left,
            Right
        }
    }
}