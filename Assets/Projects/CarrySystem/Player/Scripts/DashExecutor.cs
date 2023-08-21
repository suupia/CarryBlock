using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashExecutor : IDashExecutor
    {
        public void Setup(PlayerInfo info)
        {
            
        }

        public void Dash()
        {
            Debug.Log($"Executing Dash");
        }
    }
}