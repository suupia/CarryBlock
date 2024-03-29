using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Carry.CarrySystem.CarryScene.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using Carry.CarrySystem.Spawners.Scripts;
using Cysharp.Threading.Tasks;
using Fusion;
using Carry.Utility.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using Carry.GameSystem.Spawners.Scripts;
using Carry.GameSystem.Scripts;
using Projects.CarrySystem.Enemy;
using UnityEngine;
using VContainer;
#nullable enable

namespace Carry.GameSystem.LobbyScene.Scripts
{
    [DisallowMultipleComponent]
    public class LobbyInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        NetworkPlayerSpawner _networkPlayerSpawner = null!;
        PlayerCharacterTransporter _playerCharacterTransporter = null!;
        IMapSwitcher _lobbyMapSwitcher = null!;
        CarryInitializersReady _carryInitializersReady = null!;

        [Inject]
        public void Construct(
            NetworkPlayerSpawner networkPlayerSpawner ,
            PlayerCharacterTransporter playerCharacterTransporter,
            IMapSwitcher lobbyMapSwitcher
        )
        {
            _networkPlayerSpawner = networkPlayerSpawner;
            _playerCharacterTransporter = playerCharacterTransporter;
            _lobbyMapSwitcher = lobbyMapSwitcher;
        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));
            
            _lobbyMapSwitcher.InitSwitchMap();

            if (Runner.IsServer)
            {
                _playerCharacterTransporter.SetPlayerNumber(Runner.LocalPlayer);
                _networkPlayerSpawner.RespawnAllPlayer();
            }

            var enemySpawner = new EnemySpawner(Runner);
            enemySpawner.SpawnPrefab(new Vector3(-12f,0f,0f), Quaternion.Euler(0f, 180f, 0f));


        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if(Runner.IsClient) return;
            _networkPlayerSpawner.SpawnPlayer(player );
            
            Debug.Log($"PlayerJoined");
            _playerCharacterTransporter.SetPlayerNumber(player);
            _carryInitializersReady = FindObjectOfType<CarryInitializersReady>();
            if (_carryInitializersReady == null)
            {
                Debug.LogError($"_carryInitializersReady is null");
                return;
            }
            _carryInitializersReady.AddInitializerReady(player);


            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if(Runner.IsClient) return;
             _networkPlayerSpawner.DespawnPlayer(player);
        }

        // ボタンから呼び出す
        public void TransitionToGameScene()
        {
            if (Runner.IsClient) return;

            Debug.Log("全員が準備完了かどうかを無視し、ゲームを開始します");
            SceneTransition.TransitionSceneWithNetworkRunner(Runner, SceneName.CarryScene);

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