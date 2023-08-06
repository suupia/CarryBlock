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
        readonly MoveExecutorContainer _moveExecutorContainer;
        [Inject]
        public MainCarryPlayerFactory(
            IMapUpdater mapUpdater ,
            HoldingBlockObserver holdingBlockObserver,
            MoveExecutorContainer moveExecutorContainer
            )
        {
            _mapUpdater = mapUpdater;
            _holdingBlockObserver = holdingBlockObserver;
            _moveExecutorContainer = moveExecutorContainer;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = _moveExecutorContainer.RegularMoveExecutor;
            var blockContainer = new PlayerBlockContainer();
            var holdExe = new HoldActionExecutor(blockContainer,_mapUpdater);
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var passExe = new PassActionExecutor(blockContainer, holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe, blockContainer);
            return character;
        }
    }
}