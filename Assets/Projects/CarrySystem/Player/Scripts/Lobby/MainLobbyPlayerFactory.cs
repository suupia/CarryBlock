using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainLobbyPlayerFactory : ICarryPlayerFactory
    {
        readonly PlayerCharacterHolder _playerCharacterHolder;

        [Inject]
        public MainLobbyPlayerFactory()
        {
        }

        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExeSwitcher = new MoveExecutorSwitcher();
            var blockContainer = new PlayerHoldingObjectContainer();
            var holdExe = new EmptyHoldActionExecutor();
            var passExe = new EmptyPassActionExecutor();
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher, _playerCharacterHolder);
            var dashExe = new DashExecutor(moveExeSwitcher, onDamageExe);
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer);
            return character;
        }
    }
}