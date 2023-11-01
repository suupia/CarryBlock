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
        
        public ICharacter Create(PlayerColorType colorType)
        {
            // IMoveExecutorSwitcher
            var moveExeSwitcher = new MoveExecutorSwitcher();
            
            // IHoldActionExecutor
            var blockContainer = new PlayerHoldingObjectContainer();
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var holdExe = new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
            
            // IOnDamageExecutor
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher, _playerCharacterTransporter);

            // IDashActionExecutor
            var dashExe = new DashExecutor(moveExeSwitcher, onDamageExe);

            // IPassActionExecutor
            var passBlockMoveExe = new PassWaitExecutor();
            var passExe = new PassActionExecutor(blockContainer, passBlockMoveExe);
            
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe, onDamageExe, blockContainer);
            return character;
        }
        
        public IMoveExecutorSwitcher CreateMoveExecutorSwitcher()
        {
            return new MoveExecutorSwitcher();
        }
        
        public IHoldActionExecutor CreateHoldActionExecutor()
        {
            var blockContainer = new PlayerHoldingObjectContainer();
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            return new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
        } 
                
        public IOnDamageExecutor OnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
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