using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorSwitcher : IMoveExecutor
    {
        void SetRegularMoveExecutor();
        void SetFastMoveExecutor();
        void SetSlowMoveExecutor();
    }
}