using Carry.CarrySystem.Player.Info;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter: ICharacterMove, ICharacterAction
    {
       new void Setup(PlayerInfo info);
    }
}