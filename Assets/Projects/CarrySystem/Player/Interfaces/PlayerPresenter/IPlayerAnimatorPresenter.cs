namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerAnimatorPresenter : IPlayerBlockPresenter
    {
        void Idle();
        void Walk();
        void Dash();
    }
}