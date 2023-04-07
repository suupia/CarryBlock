using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

[DisallowMultipleComponent]
public class LobbyInitializer : NetworkSceneInitializer
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
        //後でキャッシュを取るようにして動作改善か、そもそもの仕様を変える
        // var playerUnits = networkPlayerContainer.PlayerControllers.Map(pc => pc.NowUnit).Where(unit => unit != null).ToArray();
        var playerUnits = networkPlayerContainer.PlayerControllers.Select(playerController => playerController.Unit).ToArray(); // TODO: ToArray()を消す　あと、e.SetDirectionの引数も変える
        // Array.ForEach(networkEnemyContainer.Enemies, e => e.SetDirection(playerUnits));
        foreach (var networkEnemy in networkEnemyContainer.Enemies)
        {
            networkEnemy.SetDirection(playerUnits);
        }
    }
    
}