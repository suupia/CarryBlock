using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPassActionExecutor
    {
        void SetHoldPresenter(IPlayerBlockPresenter presenter);

        void Setup(PlayerInfo info);
        void Reset();
        void PassAction();

        bool CanReceivePass();

        void PassBlock();
        void ReceivePass();
    }
}