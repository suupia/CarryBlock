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
        NetworkPlayerContainer _networkPlayerContainer = new();
        NetworkEnemyContainer _networkEnemyContainer = new();
        PlayerSpawner _playerSpawner;
        EnemySpawner _enemySpawner;
        [SerializeField] NetworkWaveTimer _networkWaveTimer;
        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            // Runner.StartGame() if it has not been run.
            await runnerManager.AttemptStartScene("GameSceneTestRoom");
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner), cancellationToken: new CancellationToken());
            
            // Domain
            _playerSpawner = new PlayerSpawner(Runner);
            _enemySpawner = new EnemySpawner(Runner);
            _networkWaveTimer.Init();
            Runner.AddSimulationBehaviour(_networkWaveTimer);


            if (Runner.IsServer)
            {
                _playerSpawner.RespawnAllPlayer(_networkPlayerContainer);
            }
    
            if (Runner.IsServer)
            {
                _networkEnemyContainer.MaxEnemyCount = 5;
                var _ = _enemySpawner.StartSimpleSpawner(0, 5f,_networkEnemyContainer);
            }
        }
        
        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer)
            {
                SceneTransition.TransitioningScene(Runner,SceneName.LobbyScene);
            }
        }
        
        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                _playerSpawner.SpawnPlayer(player,_networkPlayerContainer);
        
                // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
            }
        }
        
        
        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer)
            {
                _playerSpawner.DespawnPlayer(player,_networkPlayerContainer);
            }
        }
     
    }
}

