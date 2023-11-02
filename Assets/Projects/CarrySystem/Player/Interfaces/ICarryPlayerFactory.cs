using Carry.CarrySystem.Player.Scripts;

namespace Carry.CarrySystem.Player.Interfaces
{
    
    /// <summary>
    /// NetworkBehaviourから切り出した処理をまとめて、ICharacterを生成する
    /// </summary>
    public interface ICarryPlayerFactory
    {
        ICharacter Create(PlayerColorType colorType);
        
        // Since it is just "new" in the concrete class, leave it as the default implementation for now.
        public PlayerHoldingObjectContainer CreatePlayerHoldingObjectContainer()
        {
            return new PlayerHoldingObjectContainer();
        }

        public IMoveExecutorSwitcher CreateMoveExecutorSwitcher();

        public IHoldActionExecutor CreateHoldActionExecutor(PlayerHoldingObjectContainer blockContainer);

        public IOnDamageExecutor OnDamageExecutor(IMoveExecutorSwitcher moveExecutorSwitcher);

        public IDashExecutor CreateDashExecutor(IMoveExecutorSwitcher moveExecutorSwitcher,
            IOnDamageExecutor onDamageExecutor);

        public IPassActionExecutor CreatePassActionExecutor(PlayerHoldingObjectContainer blockContainer);
    }
}