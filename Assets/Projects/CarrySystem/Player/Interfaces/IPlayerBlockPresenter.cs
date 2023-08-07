using Carry.CarrySystem.Block.Interfaces;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerBlockPresenter
    {
        void PickUpBlock(IBlock block);
        void PutDownBlock();
        void ReceiveBlock();

        void PassBlock();
    }
}