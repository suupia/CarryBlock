using Carry.CarrySystem.Player.Interfaces;

namespace Carry.CarrySystem.Player.Scripts
{
    public class DefaultCarryPlayerFactory : ICarryPlayerFactory
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