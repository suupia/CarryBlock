using Fusion;
using System;
using System.Linq;
using UnityEngine;

namespace Main
{
    public interface IUnitAttack
    {
        bool AttemptAttack();
        float AttackCooldown();
    }


    public class UnitShooter : IUnitAttack
    {
        PlayerInfo _info;

        public float AttackCooldown() => 0.5f;

        public UnitShooter(PlayerInfo info)
        {
            _info = info;
        }

        public bool AttemptAttack()
        {
            Collider[] colliders = Physics.OverlapSphere(_info.playerObj.transform.position, _info.rangeRadius);
            var enemys = colliders.Where(collider => collider.CompareTag("Enemy"))
                .Select(collider => collider.gameObject);

            if (enemys.Any())
            {
                ShootEnemy(enemys.First());
                return true;
            }
            else
            {
                return false;
            }
        }

        void ShootEnemy(GameObject targetEnemy)
        {
            // Debug.Log($"ShootEnemy() targetEnemy:{targetEnemy}");
            var bulletInitPos =
                _info.bulletOffset * (targetEnemy.gameObject.transform.position - _info.playerObj.transform.position)
                .normalized + _info.playerObj.transform.position;
            var bulletObj = _info._runner.Spawn(_info.bulletPrefab, bulletInitPos, Quaternion.identity, PlayerRef.None);
            var bullet = bulletObj.GetComponent<NetworkBulletController>();
            bullet.Init(targetEnemy);
        }
    }
    
    public class NoneAttack : IUnitAttack
    {
        public float AttackCooldown() => 0.5f;

        public bool AttemptAttack()
        {
            return false;
        }
    }
}