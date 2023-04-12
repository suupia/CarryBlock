using System;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Animations.Scripts
{
    public class TestInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        private NetworkRunnerManager _runnerManager;

        [SerializeField] private NetworkPrefabRef testController;
        [SerializeField] private string overrideSessionName;
        private async void Start()
        {
            _runnerManager = FindObjectOfType<NetworkRunnerManager>();
            await _runnerManager.StartScene(overrideSessionName);
            _runnerManager.Runner.AddCallbacks(new LocalInputPoller());
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