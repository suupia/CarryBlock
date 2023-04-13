using UnityEngine;

namespace Animations.Scripts
{
    internal interface IAnimatorMove
    {
        /// <summary>
        /// Needs some param
        /// </summary>
        /// <param name="direction">Used for manage body animation</param>
        void OnMove(Vector3 direction);
    }

    internal interface IAnimatorDead
    {
        void OnDead();
    }

    internal interface IAnimatorSpawn
    {
        void OnSpawn();
    }

    internal interface IAnimatorAttack
    {
        void OnAttack();
    }

    internal interface IAnimatorPlayerUnit: IAnimatorAttack, IAnimatorDead, IAnimatorMove, IAnimatorSpawn
    {
        void OnMainAction();
    }
}