using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CarryPlayerFactory : ICarryPlayerFactory
    {
        IObjectResolver _resolver;
        [Inject]
        public CarryPlayerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        public ICharacter Create(PlayerColorType colorType)
        {
            // ToDo: switch文で分ける
            var move = new QuickTurnMove();
            var action = new HoldAction(_resolver);
            var character = new Character(move, action);
            return character;
        }
    }
}