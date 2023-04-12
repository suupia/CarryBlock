using System;
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

        public void OnMove(Vector3 direction, Transform target = null)
        {
            if (direction == Vector3.zero) return;
            _info.Animator.SetFloat(Speed, direction.magnitude);
        }

        public void OnAttack(Transform target = null)
        {
            if (target == null) return;
            var pos = target.position;
            var transform = _info.ShooterObject.transform;
            //ignore y value
            transform.LookAt(new Vector3(pos.x, transform.position.y, pos.z));
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
