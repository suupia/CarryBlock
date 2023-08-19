using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter: IMoveExecutorContainer, IHoldActionExecutor , IPassActionExecutor
    {
       new void Setup(PlayerInfo info);
       
       new void Reset();
       void SetHoldPresenter(IPlayerBlockPresenter presenter);
       
       PlayerBlockContainer PlayerBlockContainer{get;}
       PlayerPresenterContainer PresenterContainer{get;}

    }
}