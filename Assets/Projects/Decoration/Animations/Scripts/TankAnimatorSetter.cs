using System;
using Decoration;
using UnityEngine;

namespace Animations
{
    public class TankAnimatorSetter: IDecorationPlayer
    {
        private readonly GameObject _gameObject;
        private readonly Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        public TankAnimatorSetter(GameObject gameObject)
        {
            _gameObject = gameObject;
            _animator = gameObject.GetComponentInChildren<Animator>();
        }

        public void OnMove()
        {
            // _animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnDamage()
        {
            
        }

        public void OnAttack(bool value = true)
        {
        }

        public void OnMainAction()
        {
            
        }

        public void OnDead()
        {
            
        }

        public void OnSpawn()
        {
            
        }
    }

}
