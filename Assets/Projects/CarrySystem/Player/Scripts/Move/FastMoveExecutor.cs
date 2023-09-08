using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class FastMoveExecutor : IMoveExecutor
    {
        PlayerInfo _info = null!;
        readonly float _acceleration = 30f;
        readonly float _maxVelocity = 3f; // CorrectlyStopの半分以下
        readonly float _stoppingForce = 5f;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Move(Vector3 input)
        {
            // 早い動き
            // ダッシュの動き
        }
    }
}