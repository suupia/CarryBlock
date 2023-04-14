using System;
using Animations.Scripts;
using Network.AnimatorSetter.Info;
using UnityEngine;

namespace Network.AnimatorSetter
{

    
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
