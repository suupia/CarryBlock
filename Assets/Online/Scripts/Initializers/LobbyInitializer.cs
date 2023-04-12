using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

[DisallowMultipleComponent]
public class LobbyInitializer : SceneInitializer
{
    async void  Start()
    {
        await runnerManager.StartScene("LobbySceneTestRoom");
        base.Init();

        await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner), cancellationToken: token); 


        
        if (Runner.IsServer)
        {
            playerSpawner.RespawnAllPlayer(networkPlayerContainer);
        }

        if (Runner.IsServer)
        {
            networkEnemyContainer.MaxEnemyCount = 5;
            var _ = enemySpawner.StartSimpleSpawner(0, 5f,networkEnemyContainer);
        }
    }

    // ボタンから呼び出す
    public void SetActiveGameScene()
    {
        if (Runner.IsServer)
        {
            if (networkPlayerContainer.IsAllReady)
            {
                enemySpawner.CancelSpawning();
                phaseManager.SetPhase(Phase.Starting);
            }
        }
    }

    public override void FixedUpdateNetwork()
    {

    }
    
}