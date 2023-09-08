using System;
using Carry.CarrySystem.Block.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallControllerNet : NetworkBehaviour
    {
        readonly float _speed = 5f;

        Vector3 _direction;
        
        public void Init(CannonBlock.Kind kind)
        {
            Debug.Log($"CannonBallControllerNet.Init() called");

            _direction = kind switch
            {
                CannonBlock.Kind.Up => Vector3.forward,
                CannonBlock.Kind.Down => Vector3.back,
                CannonBlock.Kind.Left => Vector3.left,
                CannonBlock.Kind.Right => Vector3.right,
                _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
            };
        }

        void Update()
        {
            transform.position += _direction * Time.deltaTime * _speed;
        }
    }
}