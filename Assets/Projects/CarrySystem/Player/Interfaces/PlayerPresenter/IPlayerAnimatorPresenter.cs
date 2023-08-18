using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerAnimatorPresenter : IPlayerBlockPresenter
    {
        void SetAnimator(Animator animator);
        void Idle();
        void Walk();
        void Dash();
    }
}