using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Scripts;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter:
        IMoveExecutorSwitcher, 
        IHoldActionExecutor, 
        IOnDamageExecutor,
        IDashExecutor,
        IPassActionExecutor
    {
       new void Setup(PlayerInfo info);
       
       new void Reset();

       PlayerHoldingObjectContainer PlayerHoldingObjectContainer{get;}
       
       // Use "new" to avoid "Ambiguous invocation" error
       public new void SetPlayerBlockPresenter(IPlayerBlockPresenter presenter);
       public new void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter);
       public new void SetDashEffectPresenter(DashEffectPresenter presenter);
       public new void SetReviveEffectPresenter(ReviveEffectPresenter presenter);
    }
}