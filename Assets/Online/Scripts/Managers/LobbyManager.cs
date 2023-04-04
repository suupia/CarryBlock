using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class LobbyManager : NetworkSceneManager
{
    async void Start()
    {
        await base.Init();

        if (Runner.IsServer)
        {
            playerSpawner.RespawnAllPlayer();
        }

        if (Runner.IsServer)
        {
            enemySpawner.MaxEnemyCount = 5;
            var _ = enemySpawner.StartSimpleSpawner(0, 5f);
        }
    }

    // ボタンから呼び出す
    public void SetActiveGameScene()
    {
        if (Runner.IsServer)
        {
            if (playerSpawner.IsAllReady)
            {
                phaseManager.SetPhase(Phase.Starting);
            }
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