using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Main
{
    public class EnemySpawner
    {
        readonly NetworkRunner _runner;
        readonly CancellationTokenSource _cts = new();
        readonly CancellationToken _token;
        readonly NetworkBehaviourSpawner<NetworkEnemyController> _enemySpawner;
        const float spawnRadius = 50;
    
        public EnemySpawner(NetworkRunner runner)
        {
            _runner = runner;
            _enemySpawner = new NetworkBehaviourSpawner<NetworkEnemyController>(runner,
                new PrefabLoaderFromResources<NetworkEnemyController>("Prefabs/Enemys"));
            _token = _cts.Token;
        }
    
        public void CancelSpawning()
        {
            _cts.Cancel();
        }
    
        public async UniTask StartSimpleSpawner(int index, float interval, NetworkEnemyContainer enemyContainer)
        {
            while (true)
            {
                SpawnEnemy(enemyContainer);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: _token);
            }
        }
    
        void SpawnEnemy(NetworkEnemyContainer enemyContainer)
        {
            if (enemyContainer.Enemies.Count() >= enemyContainer.MaxEnemyCount) return;
            var x = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
            var z = UnityEngine.Random.Range(-spawnRadius, spawnRadius);
            var position = new Vector3(x, 1, z);
            var networkObject = _enemySpawner.Spawn("Enemy", position, Quaternion.identity, PlayerRef.None);
            var enemy = networkObject.GetComponent<NetworkEnemyController>();
            enemy.onDespawn += () => enemyContainer.RemoveEnemy(enemy);
            enemyContainer.AddEnemy(enemy);
        }
    }
    
    public class NetworkEnemyContainer
    {
        List<NetworkEnemyController> enemies = new();
        public int MaxEnemyCount { get; set; } = 128;
        public IEnumerable<NetworkEnemyController> Enemies => enemies;
    
        public void AddEnemy(NetworkEnemyController enemyController)
        {
            enemies.Add(enemyController);
        }
    
        public void RemoveEnemy(NetworkEnemyController enemyController)
        {
            enemies.Remove(enemyController);
        }
    }
}
