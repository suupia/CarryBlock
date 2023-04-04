using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;


public class NetworkPlayerContainer : SimulationBehaviour
{
    List<NetworkPlayerController> playerControllers = new();
    public NetworkPlayerController[] PlayerControllers => playerControllers.ToArray();
    public bool IsAllReady => PlayerControllers.All(pc => pc.IsReady);

    public void AddPlayer(NetworkPlayerController playerController)
    {
        playerControllers.Add(playerController);
    }

    public void RemovePlayer(NetworkPlayerController playerController)
    {
        playerControllers.Remove(playerController);
    }
    
    // スポーンの処理は切り出して、Containerは参照をもつだけにしたい。

    //Will be called outer NetworkBehaviour
    
    // public void RespawnAllPlayer()
    // {
    //     foreach (var player in Runner.ActivePlayers)
    //     {
    //         DeSpawnPlayer(player);
    //         SpawnPlayer(player);
    //     }
    // }
    //
    // public void SpawnPlayer(PlayerRef player)
    // {
    //     Debug.Log("Spawning Player");
    //     var spawnPosition = new Vector3(0, 1, 0);
    //     var playerObject = Runner.Spawn(playerControllerPrefab, spawnPosition, Quaternion.identity, player);
    //     Runner.SetPlayerObject(player, playerObject.Object);
    //     //TODO: Set AoI
    //
    //     playerControllers.Add(playerObject.GetComponent<NetworkPlayerController>());
    // }
    //
    // public void DeSpawnPlayer(PlayerRef player)
    // {
    //     if (Runner.TryGetPlayerObject(player, out var networkObject))
    //     {
    //         var pc = networkObject.GetComponent<NetworkPlayerController>();
    //         playerControllers.Remove(pc);
    //         Runner.Despawn(networkObject);
    //         Runner.SetPlayerObject(player, null);
    //     }
    // }
}

public class NetworkPlayerSpawner
{
    NetworkRunner runner;
    // NetworkPlayerController playerControllerPrefab;
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
        // return playerControllers.Append(playerObject.GetComponent<NetworkPlayerController>());
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
