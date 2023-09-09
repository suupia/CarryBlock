using System;
using Carry.CarrySystem.Block.Scripts;
using Fusion;
using UnityEngine;

namespace Carry.CarrySystem.Gimmick.Scripts
{
    public class CannonBallControllerNet : NetworkBehaviour
    {
        readonly float _speed = 5f;
        readonly float _lifeTime = 10f;

        Vector3 _direction;
        float _timer;
        
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

       public override  void FixedUpdateNetwork()
        {
            // 生存時間
            _timer += Time.deltaTime;
            if (_timer > _lifeTime)
            {
               Runner.Despawn(Object);
            }
            
            // 移動
            transform.position += _direction * Time.deltaTime * _speed;
            
            
        }
    }
}