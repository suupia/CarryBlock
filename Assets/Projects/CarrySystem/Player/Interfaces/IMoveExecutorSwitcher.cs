#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcher : IMoveExecutor
    {
        public void AddMoveRecord<T>() where T : IMoveRecord;
        public void RemoveRecord<T>() where T : IMoveRecord;
    }
}