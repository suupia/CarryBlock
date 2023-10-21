using Carry.Utility.Interfaces;
using Carry.Utility.Scripts;
using Fusion;
using UnityEngine;

#nullable enable

namespace Projects.CarrySystem.Enemy
{
    public class EnemySpawner
    {
        readonly NetworkRunner _runner;
        readonly IPrefabLoader<EnemyControllerNet> _enemyPrefabSpawner;
        
        public EnemySpawner(NetworkRunner runner)
        {
            _runner = runner;
            _enemyPrefabSpawner =
                new PrefabLoaderFromAddressable<EnemyControllerNet>("Prefabs/Enemy/EnemyControllerNet");
        }
        
        public EnemyControllerNet SpawnPrefab(Vector3 position, Quaternion rotation)
        {
            var enemy = _enemyPrefabSpawner.Load();
            return _runner.Spawn(enemy, position, rotation, PlayerRef.None);
        }
    }
}