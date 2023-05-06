using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Main
{
    public class GameInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkWaveTimer _networkWaveTimer;
        readonly NetworkPlayerContainer _abstractNetworkPlayerContainer = new();
        EnemySpawner _enemySpawner;
        readonly NetworkEnemyContainer _networkEnemyContainer = new();
        NetworkPlayerSpawner _networkPlayerSpawner;

        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            // Runner.StartGame() if it has not been run.
            await runnerManager.AttemptStartScene("GameSceneTestRoom");
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
                cancellationToken: new CancellationToken());

            // Domain
            var playerPrefabSpawner = new NetworkPlayerPrefabSpawner(Runner);
            _networkPlayerSpawner = new NetworkPlayerSpawner(Runner, playerPrefabSpawner);
            _enemySpawner = new EnemySpawner(Runner);
            Runner.AddSimulationBehaviour(_networkWaveTimer);
            _networkWaveTimer.Init();


            if (Runner.IsServer) _networkPlayerSpawner.RespawnAllPlayer(_abstractNetworkPlayerContainer);

            if (Runner.IsServer)
            {
                _networkEnemyContainer.MaxEnemyCount = 5;
                var _ = _enemySpawner.StartSimpleSpawner(0, 5f, _networkEnemyContainer);
            }
        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _networkPlayerSpawner.SpawnPlayer(player, _abstractNetworkPlayerContainer);
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _networkPlayerSpawner.DespawnPlayer(player, _abstractNetworkPlayerContainer);
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer) SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }
    }
}