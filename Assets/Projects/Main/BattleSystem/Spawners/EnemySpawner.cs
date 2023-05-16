using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Main
{
    public class EnemySpawner
    {
        public record EnemySpawnerRecord
        {
            public Func<Vector3> GetCenter = () => Vector3.zero;
            /// <summary>
            /// 敵が湧く範囲。GetCenter + ランダムでこの範囲からSpawnする
            /// </summary>
            public float SpawnRadius = 50;
            public NetworkRunner Runner;
        } 
        CancellationTokenSource _cts;
        CancellationToken _token;

        readonly IPrefabSpawner<NetworkEnemyController> _enemyPrefabSpawner;

        EnemySpawnerRecord _record;
        
        public EnemySpawner(EnemySpawnerRecord record)
        {
            _record = record;
            _enemyPrefabSpawner = new NetworkEnemyPrefabSpawner(record.Runner);
        }

        public void CancelSpawning()
        {
            _cts?.Cancel();
        }

        public async UniTask StartSimpleSpawner(int index, float interval, NetworkEnemyContainer enemyContainer)
        {
            InitCancellationTokenSource();

            while (true)
            {
                SpawnEnemy(enemyContainer);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: _token);
            }
        }

        void InitCancellationTokenSource()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        void SpawnEnemy(NetworkEnemyContainer enemyContainer)
        {
            if (enemyContainer.Enemies.Count() >= enemyContainer.MaxEnemyCount) return;
            var x = Random.Range(-_record.SpawnRadius, _record.SpawnRadius);
            var z = Random.Range(-_record.SpawnRadius, _record.SpawnRadius);
            // var position = new Vector3(x, 1, z);
            var position = _record.GetCenter() + new Vector3(x, 0f, z);
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