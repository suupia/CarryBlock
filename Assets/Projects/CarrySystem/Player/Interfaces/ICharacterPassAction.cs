using Carry.CarrySystem.Block.Interfaces;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPassActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void PassAction();

        bool CanReceivePass();

        void ReceivePass(IBlockMonoDelegate block);
    }
}