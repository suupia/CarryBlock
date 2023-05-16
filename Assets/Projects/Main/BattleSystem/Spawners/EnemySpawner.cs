using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;
using Enemy;


namespace Main
{
    public class EnemySpawner
    {
        const float spawnRadius = 50;
        readonly CancellationTokenSource _cts = new();
        readonly IPrefabSpawner<NetworkEnemyController> _enemyPrefabSpawner;
        readonly NetworkRunner _runner;
        readonly CancellationToken _token;

        public EnemySpawner(NetworkRunner runner)
        {
            _runner = runner;
            _enemyPrefabSpawner = new NetworkEnemyPrefabSpawner(runner);
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
            var x = Random.Range(-spawnRadius, spawnRadius);
            var z = Random.Range(-spawnRadius, spawnRadius);
            var position = new Vector3(x, 1, z);
            var networkObject = _enemyPrefabSpawner.SpawnPrefab(position, Quaternion.identity, PlayerRef.None);
            var enemy = networkObject.GetComponent<NetworkEnemyController>();
            enemy.OnDespawn += () => enemyContainer.RemoveEnemy(enemy);
            enemyContainer.AddEnemy(enemy);
        }
    }

    public class NetworkEnemyContainer
    {
        readonly List<NetworkEnemyController> enemies = new();
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