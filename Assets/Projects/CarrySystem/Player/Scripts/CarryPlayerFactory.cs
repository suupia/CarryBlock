using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class CarryPlayerFactory
    {
        
        public ICharacter Create(PlayerColorType colorType)
        {
            // ToDo: switch文で分ける
            var move = new QuickTurnMove();
            var action = new HoldAction();
            var character = new Character(move, action);
            return character;
        }
    }
}