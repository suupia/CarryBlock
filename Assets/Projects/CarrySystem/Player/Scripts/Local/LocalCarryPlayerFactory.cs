using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using Carry.CarrySystem.Player.Scripts;
using VContainer;
#nullable enable

namespace Projects.CarrySystem.Player.Scripts.Local
{
    public class LocalCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IMapSwitcher _mapSwitcher;
        readonly HoldingBlockObserver  _holdingBlockObserver;

        [Inject]
        public LocalCarryPlayerFactory(
            IMapSwitcher mapSwitcher ,
            HoldingBlockObserver holdingBlockObserver
        )
        {
            _mapSwitcher = mapSwitcher;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public  Character CreateCharacter()
        {
            // PlayerHolderObjectContainer
            var blockContainer = new PlayerHoldingObjectContainer();
            
            // IMoveExecutorSwitcher
            var moveExecutorSwitcher = new MoveExecutorSwitcher();
            
            // IHoldActionExecutor
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var holdActionExecutor =new HoldActionExecutor(blockContainer,new PlayerNearCartHandlerNet(), _mapSwitcher);  //todo: new はエラー回避のため一時的に使用
            
            // IOnDamageExecutor
            var onDamageExecutor = new OnDamageExecutor(moveExecutorSwitcher, new PlayerCharacterTransporter());  //todo: new はエラー回避のため一時的に使用
            var dashExecutor = new DashExecutor(moveExecutorSwitcher, onDamageExecutor);
            
            // IPassActionExecutor
            var passBlockMoveExe = new PassWaitExecutor();
            var passActionExecutor = new PassActionExecutor(blockContainer, passBlockMoveExe); 
            
            var character = new Character(
                moveExecutorSwitcher,
                holdActionExecutor,
                dashExecutor,
                passActionExecutor,
                onDamageExecutor,
                blockContainer
            );
            
            return character;
        }
    }
}