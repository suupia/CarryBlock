using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class NetworkPlayerContainer
{
    List<NetworkPlayerController> playerControllers = new();
    public IEnumerable<NetworkPlayerController> PlayerControllers => playerControllers;
    public bool IsAllReady => playerControllers.All(pc => pc.IsReady);

    public void AddPlayer(NetworkPlayerController playerController)
    {
        playerControllers.Add(playerController);
    }

    public void RemovePlayer(NetworkPlayerController playerController)
    {
        playerControllers.Remove(playerController);
    }
    
}

public class NetworkPlayerSpawner
{
    NetworkRunner runner;
    public  NetworkPlayerSpawner(NetworkRunner runner)
    {
        this.runner = runner;
    }
    
    public void RespawnAllPlayer(NetworkPlayerContainer playerContainer)
    {
        foreach (var player in runner.ActivePlayers)
        {
            DeSpawnPlayer(player,playerContainer);
            SpawnPlayer(player,playerContainer);
        }
    }

    public void SpawnPlayer(PlayerRef player, NetworkPlayerContainer playerContainer)
    {
        Debug.Log("Spawning Player");
        var spawnPosition = new Vector3(0, 1, 0);
        var playerControllerPrefab = Resources.Load<NetworkPlayerController>("Prefabs/PlayerController");
        var playerObject = runner.Spawn(playerControllerPrefab, spawnPosition, Quaternion.identity, player);
        var playerController = playerObject.GetComponent<NetworkPlayerController>();
        runner.SetPlayerObject(player, playerObject.Object);
        playerContainer.AddPlayer(playerController);
    }

    public void DeSpawnPlayer(PlayerRef player,NetworkPlayerContainer playerContainer)
    {
        if (runner.TryGetPlayerObject(player, out var networkObject))
        {
            var playerController = networkObject.GetComponent<NetworkPlayerController>();
            
            playerContainer.RemovePlayer(playerController);
            runner.Despawn(networkObject);
            runner.SetPlayerObject(player, null);
        }
    }
}
