using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainLobbyPlayerFactory : ICarryPlayerFactory
    {
        [Inject]
        public MainLobbyPlayerFactory()
        {
        }

        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExeSwitcher = new MoveExecutorSwitcher();
            var blockContainer = new PlayerHoldingObjectContainer();
            var holdExe = new EmptyHoldActionExecutor();
            var dashExe = new DashExecutor();
            var passExe = new EmptyPassActionExecutor();
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher);
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer);
            return character;
        }
    }
}