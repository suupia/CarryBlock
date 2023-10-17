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
        readonly  HoldingBlockObserver  _holdingBlockObserver;
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
            var moveExeSwitcher = new MoveExecutorSwitcher();
            var blockContainer = new PlayerHoldingObjectContainer();
            var holdExe = new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var passBlockMoveExe = new PassBlockMoveExecutor();
            var passExe = new PassActionExecutor(blockContainer, holdExe, passBlockMoveExe,10, LayerMask.GetMask("Player"));
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher, _playerCharacterTransporter);
            var dashExe = new DashExecutor(moveExeSwitcher, onDamageExe);
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer);
            return character;
        }
    }
}