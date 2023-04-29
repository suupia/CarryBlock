using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Main
{
    public abstract class AbstractNetworkPlayerSpawner<T> where T : NetworkBehaviour
    {
        readonly NetworkRunner _runner;
        readonly IPrefabSpawner<T> _playerPrefabSpawner;

        public AbstractNetworkPlayerSpawner(NetworkRunner runner, IPrefabSpawner<T> playerPrefabSpawner)
        {
            _runner = runner;
            _playerPrefabSpawner = playerPrefabSpawner;
        }

        public void RespawnAllPlayer(AbstractNetworkPlayerContainer<T> playerContainer)
        {
            foreach (var player in _runner.ActivePlayers)
            {
                DespawnPlayer(player, playerContainer);
                SpawnPlayer(player, playerContainer);
            }
        }

        public void SpawnPlayer(PlayerRef player, AbstractNetworkPlayerContainer<T> playerContainer)
        {
            Debug.Log("Spawning Player");
            var spawnPosition = new Vector3(0, 5, 0);
            var playerController = _playerPrefabSpawner.SpawnPrefab(spawnPosition, Quaternion.identity, player);
            _runner.SetPlayerObject(player, playerController.Object);
            playerContainer.AddPlayer(playerController);
        }

        public void DespawnPlayer(PlayerRef player, AbstractNetworkPlayerContainer<T> playerContainer)
        {
            if (_runner.TryGetPlayerObject(player, out var networkObject))
            {
                var playerController = networkObject.GetComponent<T>();

                playerContainer.RemovePlayer(playerController);
                _runner.Despawn(networkObject);
                _runner.SetPlayerObject(player, null);
            }
        }
    }
    
    // A class implementing AbstractNetworkPlayerSpawner can be created as follows.
    public class NetworkPlayerSpawner : AbstractNetworkPlayerSpawner<NetworkPlayerController> 
    {
        public NetworkPlayerSpawner(NetworkRunner runner, IPrefabSpawner<NetworkPlayerController> playerPrefabSpawner) : base(runner, playerPrefabSpawner)
        {
        }
    }

    public class LobbyNetworkPlayerSpawner : AbstractNetworkPlayerSpawner<LobbyNetworkPlayerController>
    {
        public LobbyNetworkPlayerSpawner(NetworkRunner runner, IPrefabSpawner<LobbyNetworkPlayerController> playerPrefabSpawner) : base(runner, playerPrefabSpawner)
        {
        }
    }

    public abstract class AbstractNetworkPlayerContainer<T> where T : NetworkBehaviour
    {
        protected readonly List<T> playerControllers = new();
        public IEnumerable<T> PlayerControllers => playerControllers;

        public void AddPlayer(T networkPlayerController)
        {
            playerControllers.Add(networkPlayerController);
        }

        public void RemovePlayer(T networkPlayerController)
        {
            playerControllers.Remove(networkPlayerController);
        }
    }
    
    // A class implementing AbstractNetworkPlayerSpawner can be created as follows.
    public class NetworkPlayerContainer : AbstractNetworkPlayerContainer< NetworkPlayerController> 
    {
        
    }

    public class LobbyNetworkPlayerContainer : AbstractNetworkPlayerContainer< LobbyNetworkPlayerController> 
    {
        public bool IsAllReady => playerControllers.All(pc => pc.IsReady);
    }
}