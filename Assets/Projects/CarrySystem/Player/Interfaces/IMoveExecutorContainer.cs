using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IMoveExecutorContainer : IMoveExecutor
    {
        void SetRegularMoveExecutor();
        void SetSlowMoveExecutor();
    }
}