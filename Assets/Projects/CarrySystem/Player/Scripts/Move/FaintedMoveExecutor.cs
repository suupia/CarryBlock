#nullable enable
using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class FaintedMoveExecutor : IMoveExecutor
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
            // CannonBallなどからダメージを受けたときの動き
            // 全く動けないか、動けても遅い動き
        }
    }
}