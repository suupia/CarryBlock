using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICarryPlayerFactory
    {
        ICharacter Create(PlayerColorType colorType);
    }
}