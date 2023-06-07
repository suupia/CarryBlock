using Fusion;
using Nuts.NetworkUtility.ObjectPool.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Attack;
using UnityEngine;

namespace Nuts.BattleSystem.Player.Scripts
{
    /// <summary>
    ///     とりあえずNetworkObjectとする。
    ///     もし重かったり、無料枠を超えるようなら、TickAlignedのRPCでトリガーだけ通信する方式にする
    ///     参考：https://docs.google.com/presentation/d/1kGN7ZEleBgpXuXnUin8y67LmXrmQuAtbgu4rz3QSY6U/edit#slide=id.g1592fa1edef_0_25
    /// </summary>
    [RequireComponent(typeof(NetworkObject))]
    public class NetworkBulletController : PoolableObject
    {
        readonly float _lifeTime = 5;
        readonly float _speed = 30;
        readonly int _damage = 1;
        Rigidbody _rb;
        bool isInitialized;
        [Networked] TickTimer LifeTimer { get; set; }

        void OnTriggerEnter(Collider other)
        {
            if (!HasStateAuthority) return;

            if (other.CompareTag("Enemy"))
            {
                var enemy = other.GetComponent<IEnemyOnAttacked>();
                if (enemy == null)
                {
                    
                    Debug.LogError(
                        "The game object with the 'Enemy' tag does not have the 'IEnemyOnAttacked' component attached.");
                    return;
                }
                enemy.OnAttacked(_damage);
                DestroyBullet();
            }
        }


        public void Init(GameObject targetGameObj)
        {
            var directionVec = (targetGameObj.transform.position - transform.position).normalized;
            _rb = GetComponent<Rigidbody>();
            _rb.AddForce(_speed * directionVec, ForceMode.Impulse);
            LifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);

            isInitialized = true;
        }

        public override void FixedUpdateNetwork()
        {
            if (Object.HasStateAuthority)
                if (LifeTimer.Expired(Runner))
                    DestroyBullet();
        }

        void DestroyBullet()
        {
            Runner.Despawn(Object);
        }

        protected override void OnInactive()
        {
            if (!isInitialized) return;
            _rb.velocity = Vector3.zero;
        }
    }
}