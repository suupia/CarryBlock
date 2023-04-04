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
    protected NetworkPlayerContainer networkPlayerContainer;
    protected NetworkEnemyContainer networkEnemyContainer;
    protected PhaseManager phaseManager;
    protected NetworkPlayerSpawner playerSpawner;

    
    protected async UniTask Init()
    {
        runnerManager = FindObjectOfType<NetworkRunnerManager>();
        await runnerManager.StartScene();

        Debug.Log($"Runner:{Runner}, runnerManager.Runner:{runnerManager.Runner}");
        runnerManager.Runner.AddSimulationBehaviour(this); // Runnerに登録
        Debug.Log($"Runner:{Runner}");

        networkPlayerContainer = FindObjectOfType<NetworkPlayerContainer>();
        networkEnemyContainer = FindObjectOfType<NetworkEnemyContainer>();
        phaseManager = FindObjectOfType<PhaseManager>();

        Runner.AddSimulationBehaviour(networkPlayerContainer);
        Runner.AddSimulationBehaviour(networkEnemyContainer);
        Runner.AddSimulationBehaviour(phaseManager);
        
        // Domain
        playerSpawner = new NetworkPlayerSpawner(Runner);

    }

    public void PlayerJoined(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            // networkPlayerContainer.SpawnPlayer(player);
            playerSpawner.SpawnPlayer(player,networkPlayerContainer);
        }
    }

    public void PlayerLeft(PlayerRef player)
    {
        if (Runner.IsServer)
        {
            // networkPlayerContainer.DeSpawnPlayer(player);
            playerSpawner.DeSpawnPlayer(player,networkPlayerContainer);
        }
    }
}

