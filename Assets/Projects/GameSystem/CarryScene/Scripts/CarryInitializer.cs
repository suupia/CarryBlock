using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Cart.Scripts;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.FloorTimer.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Carry.GameSystem.Scripts;
using Carry.NetworkUtility.NetworkRunnerManager.Scripts;
using JetBrains.Annotations;
using UnityEngine.Serialization;
using VContainer.Unity;
using VContainer;
#nullable enable


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {    
        [SerializeField] FloorTimerNet floorTimerNet;
        PlayerSpawner _playerSpawner;
        IMapUpdater _entityGridMapSwitcher;
        CarryInitializersReady? _carryInitializersReady;
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(
            PlayerSpawner playerSpawner,
            IMapUpdater entityGridMapSwitcher
        )
        {
            _playerSpawner = playerSpawner;
            _entityGridMapSwitcher = entityGridMapSwitcher;
        }



        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));
            _carryInitializersReady = FindObjectOfType<CarryInitializersReady>();
            if (_carryInitializersReady == null)
            {
                Debug.LogError($"_carryInitializersReady is null");
                return;
            }
            _carryInitializersReady.SetInitializerReady(Runner.LocalPlayer);
            await UniTask.WaitUntil(() => _carryInitializersReady.IsAllInitializersReady());
            
            // Start Timer
            if (Runner.IsServer)
            {
                Runner.AddSimulationBehaviour(floorTimerNet);
                floorTimerNet.StartTimer();
            }
            
            
            // Generate map
            if (Runner.IsServer)
            {
                _entityGridMapSwitcher.InitUpdateMap(MapKey.Default, 0);
            }
            

            // PlayerのSpawn位置がマップに依存しているため、マップ生成後にPlayerを生成する
            if (Runner.IsServer) _playerSpawner.RespawnAllPlayer();

            IsInitialized = true;
            
        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _playerSpawner.SpawnPlayer(player );
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _playerSpawner.DespawnPlayer(player);
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer) SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }


    }

}
