using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainLobbyPlayerFactory : ICarryPlayerFactory
    {
        readonly IObjectResolver _resolver;
        [Inject]
        public MainLobbyPlayerFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        
        public ICharacter Create(PlayerColorType colorType)
        {
            var moveExe = new CorrectlyStopMoveExecutor();
            var blockContainer = new PlayerBlockContainer();
            var holdExe = new EmptyHoldActionExecutor();
            var passExe = new EmptyPassActionExecutor();
            var character = new Character(moveExe, holdExe, passExe, blockContainer);
            return character;
        }
    }
}