using System;
using Fusion;
using UnityEngine;

namespace Main.Tests
{
    public class TestInitializer:SimulationBehaviour
    {
        private readonly NetworkEnemyContainer _networkEnemyContainer = new();

        [SerializeField] private string overrideSessionName;
        [SerializeField] private EnemySpawnerController controller;
        
        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            controller.StartSimpleSpawner(_networkEnemyContainer);

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                controller.StartSimpleSpawner(_networkEnemyContainer);
                Debug.Log("Spawn Loop was Restarted");

            } else if (Input.GetKeyDown(KeyCode.C))
            {
                controller.CancelSpawning();
                Debug.Log("Spawn Loop was Canceled");
            }
        }
    }        
}