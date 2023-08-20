using Carry.CarrySystem.Player.Info;
using UnityEngine;

namespace Carry.CarrySystem.Player.Interfaces
{
    public interface IDashExecutor
    {
        void Setup(PlayerInfo info);
        void Dash();
    }
}