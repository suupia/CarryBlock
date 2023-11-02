using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using VContainer;

namespace Carry.CarrySystem.Player.Scripts
{
    public class MainLobbyPlayerFactory : ICarryPlayerFactory
    {
        readonly PlayerCharacterTransporter _playerCharacterTransporter;

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
            var onDamageExe = new OnDamageExecutor(moveExeSwitcher, _playerCharacterTransporter);
            var dashExe = new DashExecutor(moveExeSwitcher, onDamageExe);
            var character = new Character(moveExeSwitcher, holdExe,dashExe, passExe,onDamageExe, blockContainer);
            return character;
        }
        
        public IMoveExecutorSwitcher CreateMoveExecutorSwitcher()
        {
            return new MoveExecutorSwitcher();
        }

        public IHoldActionExecutor CreateHoldActionExecutor(PlayerHoldingObjectContainer blockContainer)
        {
            return new EmptyHoldActionExecutor();
        }

        public IOnDamageExecutor CreateOnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher)
        {
            return  new OnDamageExecutor(moveExecutorSwitcher, _playerCharacterTransporter);
        }

        public IDashExecutor CreateDashExecutor(IMoveExecutorSwitcher moveExecutorSwitcher,
            IOnDamageExecutor onDamageExecutor)
        {
            return  new DashExecutor(moveExecutorSwitcher, onDamageExecutor);
        }

        public IPassActionExecutor CreatePassActionExecutor(PlayerHoldingObjectContainer blockContainer)
        {
            return new EmptyPassActionExecutor();
        }
    }
}