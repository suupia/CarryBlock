using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SimulationBehaviour
{
    [SerializeField] NetworkRunnerManager runnerManager;
    MyFusion.PlayerSpawner playerSpawner;
    MyFusion.EnemySpawner enemySpawner;
    PhaseManager phaseManager;
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

    }
    public void SetActiveLobbyScene()
    {
        if (Object.HasStateAuthority)
        {
            phaseManager.SetPhase(Phase.Ending);
            enemySpawner.MaxEnemyCount = 128;
        }
    }
}
