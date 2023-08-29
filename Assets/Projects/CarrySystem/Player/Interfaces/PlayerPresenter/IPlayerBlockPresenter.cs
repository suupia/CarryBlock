using Carry.CarrySystem.Block.Interfaces;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPlayerBlockPresenter
    {
        void PickUpBlock(IBlockMonoDelegate block);
        void PutDownBlock();
        void ReceiveBlock(IBlockMonoDelegate block);
        void PassBlock();

    }
}