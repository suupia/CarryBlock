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
            // networkPlayerContainer.RespawnAllPlayer();
            playerSpawner.RespawnAllPlayer(networkPlayerContainer);
        }

        if (Runner.IsServer)
        {
            networkEnemyContainer.MaxEnemyCount = 5;
            var _ = networkEnemyContainer.StartSimpleSpawner(0, 5f);
        }
    }

    // ボタンから呼び出す
    public void SetActiveGameScene()
    {
        if (Runner.IsServer)
        {
            if (networkPlayerContainer.IsAllReady)
            {
                phaseManager.SetPhase(Phase.Starting);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        //後でキャッシュを取るようにして動作改善か、そもそもの仕様を変える
        // var playerUnits = networkPlayerContainer.PlayerControllers.Map(pc => pc.NowUnit).Where(unit => unit != null).ToArray();
        var playerUnits = networkPlayerContainer.PlayerControllers.Select(playerController => playerController.NowUnit).ToArray(); // TODO: ToArray()を消す　あと、e.SetDirectionの引数も変える
        Array.ForEach(networkEnemyContainer.Enemies, e => e.SetDirection(playerUnits));
    }
    
}