using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Nuts.BattleSystem.Scripts;
using Nuts.Utility.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.BattleSystem.Spawners.Scripts;
using UnityEngine;
using VContainer;

namespace Nuts.BattleSystem.LobbyScene.Scripts
{
    [DisallowMultipleComponent]
    public class LobbyInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        LobbyNetworkPlayerContainer _lobbyNetworkPlayerContainer;
        NetworkEnemyContainer _networkEnemyContainer;
        EnemySpawner _enemySpawner;
        LobbyNetworkPlayerSpawner _lobbyNetworkPlayerSpawner;

        [Inject]
        public void Construct(LobbyNetworkPlayerSpawner lobbyNetworkPlayerSpawner,
            LobbyNetworkPlayerContainer lobbyNetworkPlayerContainer,
            EnemySpawner enemySpawner, NetworkEnemyContainer networkEnemyContainer)
        {
            _lobbyNetworkPlayerSpawner = lobbyNetworkPlayerSpawner;
            _lobbyNetworkPlayerContainer = lobbyNetworkPlayerContainer;
            _enemySpawner = enemySpawner;
            _networkEnemyContainer = networkEnemyContainer;
        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));
            

            if (Runner.IsServer) _lobbyNetworkPlayerSpawner.RespawnAllPlayer(_lobbyNetworkPlayerContainer);

            if (Runner.IsServer)
            {
                _networkEnemyContainer.MaxEnemyCount = 5;
                var _ = _enemySpawner.StartSimpleSpawner(0, 5f, _networkEnemyContainer);
            }
        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _lobbyNetworkPlayerSpawner.SpawnPlayer(player, _lobbyNetworkPlayerContainer);
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _lobbyNetworkPlayerSpawner.DespawnPlayer(player, _lobbyNetworkPlayerContainer);
        }

        // ボタンから呼び出す
        public void TransitionToGameScene()
        {
            if (Runner.IsServer)
            {
                Debug.Log("全員が準備完了かどうかを無視し、ゲームを開始します");
                SceneTransition.TransitioningScene(Runner, SceneName.CarryScene);
                
                // if (_lobbyNetworkPlayerContainer.IsAllReady)
                // {
                //     _enemySpawner.CancelSpawning();
                //     SceneTransition.TransitioningScene(Runner, SceneName.CarryScene);
                // }
                // else
                // {
                //     Debug.Log("Not All Ready");
                // }
            }
        }
    }
}