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
        PlayerSpawner _playerSpawner = null!;
        PlayerCharacterTransporter _playerCharacterTransporter = null!;
        IMapUpdater _lobbyMapUpdater = null!;
        CarryInitializersReady _carryInitializersReady = null!;

        [Inject]
        public void Construct(
            PlayerSpawner playerSpawner ,
            PlayerCharacterTransporter playerCharacterTransporter,
            IMapUpdater lobbyMapUpdater
        )
        {
            _playerSpawner = playerSpawner;
            _playerCharacterTransporter = playerCharacterTransporter;
            _lobbyMapUpdater = lobbyMapUpdater;
        }

        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));
            
            _lobbyMapUpdater.InitUpdateMap(MapKey.Default,-1); // -1が初期マップ

            if (Runner.IsServer)
            {
                _playerCharacterTransporter.SetIndex(Runner.LocalPlayer);
                _playerSpawner.RespawnAllPlayer();
            }

            var enemySpawner = new EnemySpawner(Runner);
            enemySpawner.SpawnPrefab(Vector3.zero, Quaternion.identity);


        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _playerSpawner.SpawnPlayer(player );
            
            Debug.Log($"PlayerJoined");
            _playerCharacterTransporter.SetIndex(player);
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