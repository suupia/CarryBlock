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