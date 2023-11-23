#nullable enable

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcherNew : IMoveExecutor
    {
        public void AddMoveRecord<T>() where T : IMoveRecord;
        public void RemoveRecord<T>() where T : IMoveRecord;
    }
}