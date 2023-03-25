using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    protected MyFusion.PlayerSpawner playerSpawner;
    protected MyFusion.EnemySpawner enemySpawner;
    protected PhaseManager phaseManager;

    public override void Spawned()
    {
        playerSpawner = FindObjectOfType<MyFusion.PlayerSpawner>();
        enemySpawner = FindObjectOfType<MyFusion.EnemySpawner>();
        phaseManager = FindObjectOfType<PhaseManager>();


        if (Object.HasStateAuthority)
        {
            playerSpawner.RespawnAllPlayer();
        }
    }
    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerSpawner.SpawnPlayer(player);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerSpawner.DespawnPlayer(player);
        }
    }

}
