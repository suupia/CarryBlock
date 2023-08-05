namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerBlockPresenter
    {
        void PickUpBlock();
        void PutDownBlock();
        void ReceiveBlock();

        void PassBlock();
    }
}