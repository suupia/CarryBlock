using System;
using Network.AnimatorSetter.Info;
using UnityEngine;

namespace Animations.Scripts
{
    public class PlaneAnimatorSetter: IAnimatorPlayerUnit
    {
        private enum BodyStates
        {
            Normal = 0, Left, Right
        }
        
        private readonly PlaneAnimatorSetterInfo _info;
        private static readonly int BodyState = Animator.StringToHash("BodyState");

        public PlaneAnimatorSetter(PlaneAnimatorSetterInfo info)
        {
            _info = info;
        }


        public void OnAttack(Transform target = null)
        {
        }

        public void OnDead()
        {
        }

        public void OnMove(Vector3 direction, Transform target = null)
        {
            // Debug.Log(direction);

            //Get horizontal input.
            var horizontal = direction.x;

            var bodyState = horizontal switch
            {
                < 0 => BodyStates.Left,
                > 0 => BodyStates.Right,
                _ => BodyStates.Normal
            };
            
            _info.Animator.SetInteger(BodyState, (int)bodyState);
        }

        public void OnSpawn()
        {
        }

        public void OnMainAction()
        {
        }
    }
}