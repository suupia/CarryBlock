using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainLobbyPlayerFactory : ICarryPlayerFactory
    {
        readonly IMoveExecutorContainer _moveExecutorContainer;
        [Inject]
        public MainLobbyPlayerFactory(IMoveExecutorContainer moveExecutorContainer)
        {
            _moveExecutorContainer = moveExecutorContainer;
        }

        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = _moveExecutorContainer;
            var blockContainer = new PlayerBlockContainer();
            var holdExe = new EmptyHoldActionExecutor();
            var passExe = new EmptyPassActionExecutor();
            var character = new Character(moveExe, holdExe, passExe, blockContainer);
            return character;
        }
    }
}