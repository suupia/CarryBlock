using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;

[DisallowMultipleComponent]
public class LobbyManager : NetworkSceneManager,ISceneLoadStart
{
    // public void  SceneLoadStart()
    // {
    //     var _ = AAA();
    // }
    async void  Start()
    {
        await base.Init();

        Debug.Log($"IsInitialized:{IsInitialized}");
        // await UniTask.WaitUntil(() => IsInitialized, cancellationToken: token); //このawait意味ない上と同じ、　シーンの読み込みを待つawaitにする（それかRunnerの初期化だけど、これはさずがに大丈夫なはず）
        Debug.Log($"IsReady:{Runner.SceneManager.IsReady(Runner)}");
        // await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner), cancellationToken: token); 
        Debug.Log($"IsReady:{Runner.SceneManager.IsReady(Runner)}");


        
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
        var playerUnits = networkPlayerContainer.PlayerControllers.Select(playerController => playerController.NowUnit).ToArray(); // TODO: ToArray()を消す　あと、e.SetDirectionの引数も変える
        // Array.ForEach(networkEnemyContainer.Enemies, e => e.SetDirection(playerUnits));
        foreach (var networkEnemy in networkEnemyContainer.Enemies)
        {
            networkEnemy.SetDirection(playerUnits);
        }
    }
    
}