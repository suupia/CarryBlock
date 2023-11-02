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