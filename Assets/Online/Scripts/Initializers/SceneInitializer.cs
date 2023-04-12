using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public abstract class SceneInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] protected NetworkRunnerManager runnerManager;
    protected LocalInputPoller localInputPoller = new();
    protected NetworkPlayerContainer networkPlayerContainer = new();
    protected NetworkEnemyContainer networkEnemyContainer = new();
    protected PhaseManager phaseManager;
    protected PlayerSpawner playerSpawner;
    protected EnemySpawner enemySpawner;
    
    // UniTask
    CancellationTokenSource _cts = new();
    protected CancellationToken token;


    protected void Init()
    {
        runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
        runnerManager.Runner.AddCallbacks(localInputPoller);

        phaseManager = FindObjectOfType<PhaseManager>(); // Todo: PhaseManger関連の実装
        Runner.AddSimulationBehaviour(phaseManager);
        
        // Domain
        playerSpawner = new PlayerSpawner(Runner);
        enemySpawner = new EnemySpawner(Runner);

        token = _cts.Token;
    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            // var _= AsyncSpawnPlayer(player, token);
            playerSpawner.SpawnPlayer(player,networkPlayerContainer);

            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }
    }
    

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            playerSpawner.DespawnPlayer(player,networkPlayerContainer);
        }
    }

    // public void SceneLoadStart()
    // {
    //     Debug.Log($"SceneLoadStart()");
    // }
    //
    // public void SceneLoadDone()
    // {
    //     // AddSimulationBehaviour()でRunnerにコールバックが登録されているはずだから呼ばれるはず
    //     Debug.Log($"SceneLoadDone()");
    // }
}

