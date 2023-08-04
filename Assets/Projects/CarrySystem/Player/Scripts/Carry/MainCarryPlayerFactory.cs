using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        readonly  HoldingBlockObserver  _holdingBlockObserver;
        [Inject]
        public MainCarryPlayerFactory(IObjectResolver resolver , HoldingBlockObserver holdingBlockObserver)
        {
            _resolver = resolver;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = new CorrectlyStopMoveExecutor();
            var holdExe = new HoldActionExecutorExecutor(_resolver);
            _holdingBlockObserver.RegisterHoldAction(holdExe);
            var passExe = new PassActionExecutor(holdExe,10, LayerMask.GetMask("Player"));
            var character = new Character(moveExe, holdExe, passExe);
            return character;
        }
    }
}