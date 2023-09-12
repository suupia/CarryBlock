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
        [Inject]
        public MainCarryPlayerFactory(
            IMapUpdater mapUpdater ,
            HoldingBlockObserver holdingBlockObserver,
            PlayerNearCartHandlerNet playerNearCartHandler
            )
        {
            _mapUpdater = mapUpdater;
            _holdingBlockObserver = holdingBlockObserver;
            _playerNearCartHandler = playerNearCartHandler;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExeSwitcher = new MoveExecutorSwitcher();
            var blockContainer = new PlayerHoldingObjectContainer();
            var playerPresenterContainer = new PlayerPresenterContainer();
            var holdExe = new HoldActionExecutor(blockContainer,_playerNearCartHandler, _mapUpdater);
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var dashExe = new DashExecutor();
            var passExe = new PassActionExecutor(blockContainer,playerPresenterContainer, holdExe,10, LayerMask.GetMask("Player"));
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher);
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer, playerPresenterContainer);
            return character;
        }
    }
}