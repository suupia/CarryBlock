using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class DefaultCarryPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        [Inject]
        public DefaultCarryPlayerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var move = new QuickTurnMove();
            var action = new HoldAction(_resolver);
            var character = new Character(move, action);
            return character;
        }
    }
}