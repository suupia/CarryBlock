using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Projects.BattleSystem.Boss.Scripts;
using Projects.Utility.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Projects.BattleSystem.Spawners.Scripts
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

        readonly IPrefabSpawner<Monster1Controller_Net> _boss1PrefabSpawner;

        readonly Boss1SpawnerRecord _record;

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
            var boss = networkObject.GetComponent<Monster1Controller_Net>();
            boss.OnDespawn += () => enemyContainer.RemoveBoss(boss);
            var actionSelector = new RandomActionSelector(); // アクションの決定方法はランダム
            boss.Init(actionSelector);
            enemyContainer.AddBoss(boss);
        }
    }

    public class Boss1Container
    {
        readonly List<Monster1Controller_Net> bosses = new();
        public int MaxBossCount { get; set; } = 2;
        public IEnumerable<Monster1Controller_Net> Bosses => bosses;

        public void AddBoss(Monster1Controller_Net enemyController)
        {
            bosses.Add(enemyController);
        }

        public void RemoveBoss(Monster1Controller_Net enemyController)
        {
            bosses.Remove(enemyController);
        }
    }
    
    public class BossPrefabSpawner : IPrefabSpawner<Monster1Controller_Net>
    {
        readonly NetworkBehaviourPrefabSpawner<Monster1Controller_Net> _bossPrefabSpawner;

        public BossPrefabSpawner(NetworkRunner runner)
        {
            _bossPrefabSpawner = new NetworkBehaviourPrefabSpawner<Monster1Controller_Net>(runner,
                new PrefabLoaderFromResources<Monster1Controller_Net>("Prefabs/Bosses", "Boss1"));
        }

        public Monster1Controller_Net SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _bossPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}