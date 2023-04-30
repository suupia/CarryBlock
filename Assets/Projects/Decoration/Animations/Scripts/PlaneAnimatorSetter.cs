using System;
using Decoration;
using UnityEngine;

namespace Animations
{
    public class PlaneAnimatorSetter: IPlayerDecoration
    {
        private enum BodyStates
        {
            Normal = 0, Left, Right
        }
        
        private readonly GameObject _gameObject;
        private readonly Animator _animator;
        private Vector3 _preForward = Vector3.zero;
        private static readonly int BodyState = Animator.StringToHash("BodyState");

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

        public void OnChangeForward(Vector3 forward)
        {
            // Debug.Log(direction);
            var deltaAngle = Vector3.SignedAngle(_preForward, forward, Vector3.up);

            var bodyState = deltaAngle switch
            {
                < 0 => BodyStates.Left,
                > 0 => BodyStates.Right,
                _ => BodyStates.Normal
            };
            
            _animator.SetInteger(BodyState, (int)bodyState);
            _preForward = forward;
        }
    }
}