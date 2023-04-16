using System;
using UnityEngine;

namespace Animations
{
    public struct TankAnimatorSetterInfo
    {
        public Animator Animator { get; set; }
    }
    
    public class TankAnimatorSetter: IAnimatorPlayerUnit
    {
        private readonly TankAnimatorSetterInfo _info;
        private static readonly int Speed = Animator.StringToHash("Speed");

        public TankAnimatorSetter(TankAnimatorSetterInfo info)
        {
            _info = info;
        }

        public void OnMove(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            _info.Animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnAttack()
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
