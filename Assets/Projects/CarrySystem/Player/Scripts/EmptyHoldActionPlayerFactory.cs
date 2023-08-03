using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class EmptyHoldActionPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        [Inject]
        public EmptyHoldActionPlayerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var move = new CorrectlyStopMove();
            var action = new EmptyHoldAction();
            var character = new Character(move, action);
            return character;
        }
    }
}