using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter:
        IMoveExecutorSwitcher, 
        IHoldActionExecutor, 
        IPassActionExecutor, 
        IDashExecutor,
        IOnDamageExecutor
    {
       new void Setup(PlayerInfo info);
       
       new void Reset();
       void SetHoldPresenter(IPlayerBlockPresenter presenter);
       public void SetAidKitPresenter(PlayerAidKitPresenterNet presenter);

       PlayerHoldingObjectContainer PlayerHoldingObjectContainer{get;}
       PlayerPresenterContainer PresenterContainer{get;}

    }
}