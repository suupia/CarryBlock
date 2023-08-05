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
        public MainCarryPlayerFactory(IMapUpdater mapUpdater , HoldingBlockObserver holdingBlockObserver)
        {
            _mapUpdater = mapUpdater;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = new CorrectlyStopMoveExecutor();
            var holdExe = new HoldActionExecutor(_mapUpdater);
            _holdingBlockObserver.RegisterHoldAction(holdExe);
            var passExe = new PassActionExecutor(holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe);
            return character;
        }
    }
}