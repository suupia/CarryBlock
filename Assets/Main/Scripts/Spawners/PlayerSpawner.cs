using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


public class PlayerSpawner
{
    readonly NetworkRunner _runner;
    readonly NetworkBehaviourSpawner<NetworkPlayerController> _playerSpawner;

    public PlayerSpawner(NetworkRunner runner)
    {
        _runner = runner;
        _playerSpawner = new NetworkBehaviourSpawner<NetworkPlayerController>(runner,
            new PrefabLoaderFromResources<NetworkPlayerController>("Prefabs/Players"));
    }

    public void RespawnAllPlayer(NetworkPlayerContainer playerContainer)
    {
        foreach (var player in _runner.ActivePlayers)
        {
            DespawnPlayer(player, playerContainer);
            SpawnPlayer(player, playerContainer);
        }
    }

    public void SpawnPlayer(PlayerRef player, NetworkPlayerContainer playerContainer)
    {
        Debug.Log("Spawning Player");
        var spawnPosition = new Vector3(0, 1, 0);
        var playerController = _playerSpawner.Spawn("PlayerController", spawnPosition, Quaternion.identity, player);
        _runner.SetPlayerObject(player, playerController.Object);
        playerContainer.AddPlayer(playerController);
    }

    public void DespawnPlayer(PlayerRef player, NetworkPlayerContainer playerContainer)
    {
        if (_runner.TryGetPlayerObject(player, out var networkObject))
        {
            var playerController = networkObject.GetComponent<NetworkPlayerController>();

            playerContainer.RemovePlayer(playerController);
            _runner.Despawn(networkObject);
            _runner.SetPlayerObject(player, null);
        }
    }
}

public class NetworkPlayerContainer
{
    readonly List<NetworkPlayerController> playerControllers = new();
    public IEnumerable<NetworkPlayerController> PlayerControllers => playerControllers;
    public bool IsAllReady => playerControllers.All(pc => pc.IsReady);

    public void AddPlayer(NetworkPlayerController networkPlayerController)
    {
        playerControllers.Add(networkPlayerController);
    }

    public void RemovePlayer(NetworkPlayerController networkPlayerController)
    {
        playerControllers.Remove(networkPlayerController);
    }
}