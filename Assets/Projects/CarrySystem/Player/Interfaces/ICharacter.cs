using Carry.CarrySystem.Player.Info;
namespace Carry.CarrySystem.Player.Interfaces
{
    public interface ICharacter: IMoveExecutor, IHoldActionExecutor , IPassActionExecutor
    {
       new void Setup(PlayerInfo info);
       
       new void Reset();
    }
}