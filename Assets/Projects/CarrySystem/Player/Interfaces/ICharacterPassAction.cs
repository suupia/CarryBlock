using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IPassActionExecutor
    {
        // void SetPassPresenter(IPassActionPresenter presenter);
        void Setup(PlayerInfo info);
        void Reset();
        void PassAction();
    }
}