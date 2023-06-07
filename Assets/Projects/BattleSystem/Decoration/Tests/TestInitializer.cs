using Fusion;
using NetworkUtility.NetworkRunnerManager;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Decoration.Tests
{
    public class TestInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkPrefabRef testController;
        [SerializeField] string overrideSessionName;
        NetworkRunnerManager _runnerManager;

        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
        }

        public void PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) Runner.Spawn(testController, new Vector3(0, 0, 0), null, player);
        }

        public void PlayerLeft(PlayerRef player)
        {
            if (!Runner.IsServer) return;
            if (!Runner.TryGetPlayerObject(player, out var networkObject)) return;
            Runner.Despawn(networkObject);
        }
    }
}