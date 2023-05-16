using System;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;

namespace Main.Tests
{
    public class TestInitializer:SimulationBehaviour
    {
        readonly NetworkEnemyContainer _networkEnemyContainer = new();

        [SerializeField] string overrideSessionName;
        [SerializeField] EnemySpawnerController enemySpawnerController;
        
        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            enemySpawnerController.StartSimpleSpawner(_networkEnemyContainer);

        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                enemySpawnerController.StartSimpleSpawner(_networkEnemyContainer);
                Debug.Log("Spawn Loop was Restarted");

            } else if (Input.GetKeyDown(KeyCode.C))
            {
                enemySpawnerController.CancelSpawning();
                Debug.Log("Spawn Loop was Canceled");
            }
        }
    }        
}