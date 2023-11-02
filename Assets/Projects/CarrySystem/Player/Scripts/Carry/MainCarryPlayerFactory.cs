using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IMapUpdater _mapUpdater;
        readonly HoldingBlockObserver  _holdingBlockObserver;
        readonly PlayerNearCartHandlerNet _playerNearCartHandler;
        readonly PlayerCharacterTransporter _playerCharacterTransporter;
        [Inject]
        public MainCarryPlayerFactory(
            IMapUpdater mapUpdater ,
            HoldingBlockObserver holdingBlockObserver,
            PlayerNearCartHandlerNet playerNearCartHandler,
            PlayerCharacterTransporter playerCharacterTransporter
            )
        {
            _mapUpdater = mapUpdater;
            _holdingBlockObserver = holdingBlockObserver;
            _playerNearCartHandler = playerNearCartHandler;
            _playerCharacterTransporter = playerCharacterTransporter;
        }

        public PlayerHoldingObjectContainer CreatePlayerHoldingObjectContainer()
        {
            return new PlayerHoldingObjectContainer();
        }

        public IMoveExecutorSwitcher CreateMoveExecutorSwitcher()
        {
            return new MoveExecutorSwitcher();
        }
        
        public IHoldActionExecutor CreateHoldActionExecutor(PlayerHoldingObjectContainer blockContainer)
        {
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            return new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
        } 
                
        public IOnDamageExecutor CreateOnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            return new OnDamageExecutor(moveExecutorSwitcher, _playerCharacterTransporter);
        }
        
        public IDashExecutor CreateDashExecutor(IMoveExecutorSwitcher moveExecutorSwitcher, IOnDamageExecutor onDamageExecutor)
        {
            return new DashExecutor(moveExecutorSwitcher, onDamageExecutor);
        }
        
        public IPassActionExecutor CreatePassActionExecutor(PlayerHoldingObjectContainer blockContainer)
        {
            var passBlockMoveExe = new PassWaitExecutor();
            return new PassActionExecutor(blockContainer, passBlockMoveExe);
        }


    }
}