using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Boss;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Random = UnityEngine.Random;
using Enemy;


namespace Main
{

    public class Boss1Spawner
    {
        public record Boss1SpawnerRecord
        {
            public Func<Vector3> GetCenter = () => Vector3.zero;
            /// <summary>
            /// 敵が湧く範囲。GetCenter + ランダムでこの範囲からSpawnする
            /// </summary>
            public float SpawnRadius = 50;
        } 
        CancellationTokenSource _cts;
        CancellationToken _token;

        readonly IPrefabSpawner<Boss1Controller_Net> _boss1PrefabSpawner;

        Boss1SpawnerRecord _record;

        public Boss1Spawner(NetworkRunner runner)
        {
            _record = new Boss1SpawnerRecord();
            _boss1PrefabSpawner = new BossPrefabSpawner(runner);
        }
        
        public Boss1Spawner(NetworkRunner runner, Boss1SpawnerRecord record)
        {
            _record = record;
            _boss1PrefabSpawner = new BossPrefabSpawner(runner);
        }

        public void CancelSpawning()
        {
            _cts?.Cancel();
        }

        public async UniTask StartSimpleSpawner(int index, float interval, Boss1Container enemyContainer)
        {
            InitCancellationTokenSource();

            while (true)
            {
                SpawnBoss(enemyContainer);
                await UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: _token);
            }
        }

        void InitCancellationTokenSource()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }

        void SpawnBoss(Boss1Container enemyContainer)
        {
            if (enemyContainer.Bosses.Count() >= enemyContainer.MaxBossCount) return;
            var x = Random.Range(-_record.SpawnRadius, _record.SpawnRadius);
            var z = Random.Range(-_record.SpawnRadius, _record.SpawnRadius);
            // var position = new Vector3(x, 1, z);
            var position = _record.GetCenter() + new Vector3(x, 0f, z);
            var networkObject = _boss1PrefabSpawner.SpawnPrefab(position, Quaternion.identity, PlayerRef.None);
            var boss = networkObject.GetComponent<Boss1Controller_Net>();
            boss.OnDespawn += () => enemyContainer.RemoveBoss(boss);
            var actionSelector = new RandomAttackSelector(); // アクションの決定方法はランダム
            boss.Init(actionSelector);
            enemyContainer.AddBoss(boss);
        }
    }

    public class Boss1Container
    {
        readonly List<Boss1Controller_Net> bosses = new();
        public int MaxBossCount { get; set; } = 3;
        public IEnumerable<Boss1Controller_Net> Bosses => bosses;

        public void AddBoss(Boss1Controller_Net enemyController)
        {
            bosses.Add(enemyController);
        }

        public void RemoveBoss(Boss1Controller_Net enemyController)
        {
            bosses.Remove(enemyController);
        }
    }
    
    public class BossPrefabSpawner : IPrefabSpawner<Boss1Controller_Net>
    {
        readonly NetworkBehaviourPrefabSpawner<Boss1Controller_Net> _bossPrefabSpawner;

        public BossPrefabSpawner(NetworkRunner runner)
        {
            _bossPrefabSpawner = new NetworkBehaviourPrefabSpawner<Boss1Controller_Net>(runner,
                new PrefabLoaderFromResources<Boss1Controller_Net>("Prefabs/Bosses"), "Boss1");
        }

        public Boss1Controller_Net SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _bossPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}