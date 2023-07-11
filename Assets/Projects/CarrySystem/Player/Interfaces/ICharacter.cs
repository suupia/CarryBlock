using Carry.CarrySystem.Player.Info;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter: ICharacterMove, ICharacterHoldAction
    {
       new void Setup(PlayerInfo info);
    }
}