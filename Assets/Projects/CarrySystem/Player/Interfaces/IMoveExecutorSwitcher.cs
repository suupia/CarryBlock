#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcher : IMoveExecutor
    {
        public void AddMoveRecord<T>() where T : IMoveChainable;
        public void RemoveRecord<T>() where T : IMoveChainable;
    }
}