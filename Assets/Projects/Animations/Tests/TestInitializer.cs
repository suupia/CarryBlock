using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using Main;

namespace Animations.Tests
{
    public class TestInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
         NetworkRunnerManager _runnerManager;

        [SerializeField]  NetworkPrefabRef testController;
        [SerializeField]  string overrideSessionName;
        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner

        }

        public void PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                Runner.Spawn(testController, new Vector3(0,0,0), null, player);
            }
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (!Runner.IsServer) return;
            if (!Runner.TryGetPlayerObject(player, out var networkObject)) return;
            Runner.Despawn(networkObject);
        }
    }
}