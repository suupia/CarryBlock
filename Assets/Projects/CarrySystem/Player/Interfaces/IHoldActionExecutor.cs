using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IHoldActionExecutor
    {
        void Setup(PlayerInfo info);
        void Reset();
        void HoldAction();
    }
}