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
            var moveExe = new MoveExecutorSwitcher();
            var blockContainer = new PlayerBlockContainer();
            var playerPresenterContainer = new PlayerPresenterContainer();
            var holdExe = new EmptyHoldActionExecutor();
            var dashExe = new DashExecutor();
            var passExe = new EmptyPassActionExecutor();
            var onDamageExe = new OnDamageExecutor();
            var character = new Character(moveExe, holdExe,dashExe, passExe,onDamageExe, blockContainer,playerPresenterContainer);
            return character;
        }
    }
}