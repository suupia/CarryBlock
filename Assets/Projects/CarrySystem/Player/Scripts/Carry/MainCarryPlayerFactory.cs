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
        [Inject]
        public MainCarryPlayerFactory(
            IMapUpdater mapUpdater ,
            HoldingBlockObserver holdingBlockObserver
            )
        {
            _mapUpdater = mapUpdater;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = new MoveExecutorContainer();
            var blockContainer = new PlayerBlockContainer();
            var holdExe = new HoldActionExecutor(blockContainer,_mapUpdater);
            _holdingBlockObserver.RegisterHoldAction(blockContainer);
            var passExe = new PassActionExecutor(blockContainer, holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe, blockContainer);
            return character;
        }
    }
}