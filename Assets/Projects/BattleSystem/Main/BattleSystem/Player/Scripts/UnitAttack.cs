using System.Linq;
using Fusion;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Player.Scripts
{
    public interface IUnitAttack
    {
        bool AttemptAttack();
        float AttackCooldown();
    }


    public class UnitShooter : IUnitAttack
    {
        readonly PlayerInfo _info;

        public UnitShooter(PlayerInfo info)
        {
            _info = info;
        }

        public float AttackCooldown()
        {
            return 0.5f;
        }

        public bool AttemptAttack()
        {
            var colliders = Physics.OverlapSphere(_info.playerObj.transform.position, _info.rangeRadius);
            var enemys = colliders.Where(collider => collider.CompareTag("Enemy"))
                .Select(collider => collider.gameObject);

            if (enemys.Any())
            {
                ShootEnemy(enemys.First());
                return true;
            }

            return false;
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
        public float AttackCooldown()
        {
            return 0.5f;
        }

        public bool AttemptAttack()
        {
            return false;
        }
    }
}