using System;
using Carry.CarrySystem.Block.Scripts;
using Carry.CarrySystem.Player.Interfaces;
using Fusion;
using UnityEngine;
#nullable enable

namespace Carry.CarrySystem.Gimmick.Scripts
{
    [RequireComponent(typeof(Collider))]
    public class CannonBallControllerNet : NetworkBehaviour
    {
        readonly float _speed = 3f;
        readonly float _lifeTime = 10f;

        Vector3 _direction;
        float _timer;
        
        Collider _collider = null!;
        
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
            if(!HasStateAuthority) return;
            
            // 移動
            transform.position += _direction * Runner.DeltaTime * _speed;
            
            // 生存時間
            _timer += Time.deltaTime;
            if (_timer > _lifeTime)
            {
               Runner.Despawn(Object);  // you have to write this code at bottom of this method
            }

        }
       
       void OnTriggerEnter(Collider other)
       {
           if(!HasStateAuthority) return;

           if (other.CompareTag("Player"))
           {
               Debug.Log($"Playerに衝突");
               var playerController = other.GetComponent<AbstractNetworkPlayerController>();
               var character = playerController.GetCharacter;
               character.OnDamage();
               Runner.Despawn(Object);
           }

           if (other.CompareTag("Wall"))
           {
               Runner.Despawn(Object);
           }
           
           if (other.CompareTag("Block"))
           {
               Runner.Despawn(Object);
           }
           
           if (other.CompareTag("CannonBlock"))
           {
               //大砲の弾が出現した瞬間に自分の大砲に衝突して自壊するのを阻止して
               //かつ他の大砲に衝突したときは消滅する
               float _cannonBallBrokenTime = 0.04f;
               if (_timer > _cannonBallBrokenTime)
               {
                   Runner.Despawn(Object);
               }
              
           }
       }
       
       
    }
}