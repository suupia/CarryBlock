using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using Nuts.BattleSystem.Enemy.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Nuts.BattleSystem.Enemy.Scripts.Spawners
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
        } 
        CancellationTokenSource _cts;
        CancellationToken _token;

        readonly IPrefabSpawner<NetworkEnemyController> _enemyPrefabSpawner;

        EnemySpawnerRecord _record;

        public EnemySpawner(NetworkRunner runner)
        {
            _record = new EnemySpawnerRecord();
            _enemyPrefabSpawner = new NetworkEnemyPrefabSpawner(runner);
        }
        
        public EnemySpawner(NetworkRunner runner, EnemySpawnerRecord record)
        {
            _record = record;
            _enemyPrefabSpawner = new NetworkEnemyPrefabSpawner(runner);
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
        public int MaxEnemyCount { get; set; } = 5;
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
    
    public class NetworkEnemyPrefabSpawner : IPrefabSpawner<NetworkEnemyController>
    {
        readonly NetworkBehaviourPrefabSpawner<NetworkEnemyController> _playerPrefabPrefabSpawner;

        public NetworkEnemyPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<NetworkEnemyController>(runner,
                new PrefabLoaderFromResources<NetworkEnemyController>("Prefabs/Enemys"), "Enemy");
        }

        public NetworkEnemyController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
    
    // The following classes specifically determine which prefab to spawn
    public class NetworkPlayerPrefabSpawner : IPrefabSpawner<NetworkPlayerController>
    {
        readonly NetworkBehaviourPrefabSpawner<NetworkPlayerController> _playerPrefabPrefabSpawner;

        public NetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<NetworkPlayerController>(runner,
                new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"), "PlayerController");
        }

        public NetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }

    public class LobbyNetworkPlayerPrefabSpawner : IPrefabSpawner<LobbyNetworkPlayerController>
    {
        readonly NetworkBehaviourPrefabSpawner<LobbyNetworkPlayerController> _playerPrefabPrefabSpawner;

        public LobbyNetworkPlayerPrefabSpawner(NetworkRunner runner)
        {
            _playerPrefabPrefabSpawner = new NetworkBehaviourPrefabSpawner<LobbyNetworkPlayerController>(runner,
                new PrefabLoaderFromResources<LobbyNetworkPlayerController>("Prefabs/Players"),
                "LobbyPlayerController");
        }

        public LobbyNetworkPlayerController SpawnPrefab(Vector3 position, Quaternion rotation, PlayerRef playerRef)
        {
            return _playerPrefabPrefabSpawner.SpawnPrefab(position, rotation, playerRef);
        }
    }
}