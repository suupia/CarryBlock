namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerAnimatorPresenter
    {
        void PickUpBlock();
        void PutDownBlock();
        void ReceiveBlock();
        void PassBlock();
        void Idle();
        void Walk();
        void Dash();
    }
}