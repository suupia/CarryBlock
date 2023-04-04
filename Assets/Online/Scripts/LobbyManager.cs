using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class LobbyManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] NetworkRunnerManager runnerManager;
    MyFusion.PlayerSpawner playerSpawner;
    MyFusion.EnemySpawner enemySpawner;
    PhaseManager phaseManager;

    // protected override void OnStartRunning()
    // {
    //     base.OnStartRunning();
    //
    //     // FusionManager クラスを取得
    //     FusionManager fusionManager = GameObject.FindObjectOfType<FusionManager>();
    //     if (fusionManager == null)
    //     {
    //         Debug.LogError("FusionManager not found.");
    //         return;
    //     }
    //
    //     // NetworkRunner を取得
    //     networkRunner = fusionManager.NetworkRunner;
    //     if (networkRunner == null)
    //     {
    //         Debug.LogError("NetworkRunner not found.");
    //         return;
    //     }
    //
    //     // NetworkRunner にアクセスする処理を実行
    //     // 例：ロビーシーンに移行する
    //     networkRunner.LoadScene("LobbyScene");
    // }

    async void Start()
    {
        await runnerManager.StartScene();

        Debug.Log($"Runner:{Runner}");
        runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
        Debug.Log($"Runner:{Runner}");


        playerSpawner = FindObjectOfType<MyFusion.PlayerSpawner>();
        enemySpawner = FindObjectOfType<MyFusion.EnemySpawner>();
        phaseManager = FindObjectOfType<PhaseManager>();

        Runner.AddSimulationBehaviour(playerSpawner);
        Runner.AddSimulationBehaviour(enemySpawner);
        Runner.AddSimulationBehaviour(phaseManager);

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
}