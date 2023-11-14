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
        
        public  Character CreateCharacter()
        {
            // PlayerHolderObjectContainer
            var blockContainer = new PlayerHoldingObjectContainer();
            
            // IMoveExecutorSwitcher
            var moveExecutorSwitcher = new MoveExecutorSwitcher();
            
            // IHoldActionExecutor
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var holdActionExecutor =new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
            
            // IOnDamageExecutor
            var onDamageExecutor = new OnDamageExecutor(moveExecutorSwitcher, _playerCharacterTransporter);
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