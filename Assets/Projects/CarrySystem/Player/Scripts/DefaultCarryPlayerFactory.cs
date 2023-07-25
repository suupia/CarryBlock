using Carry.CarrySystem.Player.Interfaces;
using VContainer;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DefaultCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        readonly  HoldingBlockObserver  _holdingBlockObserver;
        [Inject]
        public DefaultCarryPlayerFactory(IObjectResolver resolver , HoldingBlockObserver holdingBlockObserver)
        {
            _resolver = resolver;
            _holdingBlockObserver = holdingBlockObserver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var move = new CorrectlyStopMove();
            var action = new HoldAction(_resolver);
            _holdingBlockObserver.RegisterHoldAction(action);
            var character = new Character(move, action);
            return character;
        }
    }
}