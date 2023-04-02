using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public override void FixedUpdateNetwork()
    {
        //後でキャッシュを取るようにして動作改善か、そもそもの仕様を変える
        var playerUnits = playerSpawner.PlayerControllers.Map(pc => pc.NowUnit).Where(unit => unit != null).ToArray();
        Array.ForEach(enemySpawner.Enemies, e => e.SetDirection(playerUnits));
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
