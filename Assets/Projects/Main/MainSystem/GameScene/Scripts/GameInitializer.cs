using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Serialization;

namespace Main
{
    public class GameInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        NetworkPlayerContainer _abstractNetworkPlayerContainer = new();
        NetworkEnemyContainer _networkEnemyContainer = new();
        NetworkPlayerSpawner _networkPlayerSpawner;
        EnemySpawner _enemySpawner;
        [SerializeField] NetworkWaveTimer _networkWaveTimer;

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
            _networkPlayerSpawner = new NetworkPlayerSpawner(Runner,playerPrefabSpawner);
            _enemySpawner = new EnemySpawner(Runner);
            Runner.AddSimulationBehaviour(_networkWaveTimer);
            _networkWaveTimer.Init();


            if (Runner.IsServer)
            {
                _networkPlayerSpawner.RespawnAllPlayer(_abstractNetworkPlayerContainer);
            }

            if (Runner.IsServer)
            {
                _networkEnemyContainer.MaxEnemyCount = 5;
                var _ = _enemySpawner.StartSimpleSpawner(0, 5f, _networkEnemyContainer);
            }
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer)
            {
                SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
            }
        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                _networkPlayerSpawner.SpawnPlayer(player, _abstractNetworkPlayerContainer);

                // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
            }
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                _networkPlayerSpawner.DespawnPlayer(player, _abstractNetworkPlayerContainer);
            }
        }
    }
}