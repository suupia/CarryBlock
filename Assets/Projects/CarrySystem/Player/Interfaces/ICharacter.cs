using Carry.CarrySystem.Player.Info;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter: IMoveExecutorContainer, IHoldActionExecutor , IPassActionExecutor
    {
       new void Setup(PlayerInfo info);
       
       new void Reset();
       void SetHoldPresenter(IPlayerBlockPresenter presenter);

    }
}