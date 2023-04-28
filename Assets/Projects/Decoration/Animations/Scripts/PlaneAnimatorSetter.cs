using System;
using Decoration;
using UnityEngine;

namespace Animations
{
    public class PlaneAnimatorSetter: IDecorationPlayer
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


        public void OnAttack(bool value = true)
        {
        }

        public void OnDamage()
        {
        }

        public void OnDead()
        {
        }

        public void OnMove()
        {
            // Debug.Log(direction);

            var forward = _gameObject.transform.forward;
            
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

        public void OnSpawn()
        {
        }

        public void OnMainAction()
        {
        }
    }
}