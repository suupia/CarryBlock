using Carry.CarrySystem.Player.Info;
using Projects.CarrySystem.Block.Interfaces;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPassActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void PassAction();

        bool CanReceivePass();

        void ReceivePass(IBlock block);
    }
}