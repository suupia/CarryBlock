using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class NetworkSceneManager : SimulationBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] protected NetworkRunnerManager runnerManager;
    protected MyFusion.PlayerSpawner playerSpawner;
    protected MyFusion.EnemySpawner enemySpawner;
    protected PhaseManager phaseManager;
    
    protected async UniTask Init()
    {
        runnerManager = FindObjectOfType<NetworkRunnerManager>();
        await runnerManager.StartScene();

        Debug.Log($"Runner:{Runner}, runnerManager.Runner:{runnerManager.Runner}");
        runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
        Debug.Log($"Runner:{Runner}");

        playerSpawner = FindObjectOfType<MyFusion.PlayerSpawner>();
        enemySpawner = FindObjectOfType<MyFusion.EnemySpawner>();
        phaseManager = FindObjectOfType<PhaseManager>();

        Runner.AddSimulationBehaviour(playerSpawner);
        Runner.AddSimulationBehaviour(enemySpawner);
        Runner.AddSimulationBehaviour(phaseManager);
        
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
            playerSpawner.DeSpawnPlayer(player);
        }
    }
}

