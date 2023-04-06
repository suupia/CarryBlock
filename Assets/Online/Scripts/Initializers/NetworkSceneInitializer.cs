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

public abstract class NetworkSceneInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] protected NetworkRunnerManager runnerManager;
    protected LocalInputPoller localInputPoller = new();
    protected NetworkPlayerContainer networkPlayerContainer = new();
    protected NetworkEnemyContainer networkEnemyContainer = new();
    protected PhaseManager phaseManager;
    protected NetworkPlayerSpawner playerSpawner;
    protected NetworkEnemySpawner enemySpawner;
    
    // UniTask
    CancellationTokenSource cts = new();
    protected CancellationToken token;


    protected void Init()
    {
        Debug.Log($"Start {SceneManager.GetActiveScene().name} Init");

        Debug.Log($"Runner:{Runner}\nrunnerManager.Runner:{runnerManager.Runner}");
        runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
        runnerManager.Runner.AddCallbacks(localInputPoller);
        Debug.Log($"Runner:{Runner}");

        phaseManager = FindObjectOfType<PhaseManager>();

        Runner.AddSimulationBehaviour(phaseManager);
        
        // Domain
        playerSpawner = new NetworkPlayerSpawner(Runner);
        enemySpawner = new NetworkEnemySpawner(Runner);

        this.token = cts.Token;
        
        Debug.Log($"Finish {SceneManager.GetActiveScene().name} Init");
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

