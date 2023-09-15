using System.Threading;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Fusion;
using Projects.Utility.Scripts;
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
using Projects.BattleSystem.Scripts;
using Projects.BattleSystem.Spawners.Scripts;
using UnityEngine;
using VContainer;
#nullable enable

namespace Projects.BattleSystem.LobbyScene.Scripts
{
    [DisallowMultipleComponent]
    public class LobbyInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        PlayerSpawner _playerSpawner = null!;
        PlayerCharacterHolder _playerCharacterHolder = null!;

        [Inject]
        public void Construct(PlayerSpawner playerSpawner , PlayerCharacterHolder playerCharacterHolder)
        {
            _playerSpawner = playerSpawner;
            _playerCharacterHolder = playerCharacterHolder;
        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));

            if (Runner.IsServer)
            {
                _playerCharacterHolder.SetIndex(Runner.LocalPlayer);
                _playerSpawner.RespawnAllPlayer();
            }

        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _playerSpawner.SpawnPlayer(player );
            
            Debug.Log($"PlayerJoined");
            _playerCharacterHolder.SetIndex(player);

            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _playerSpawner.DespawnPlayer(player);
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