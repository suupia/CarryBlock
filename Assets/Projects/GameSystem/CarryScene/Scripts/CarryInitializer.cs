using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Interfaces;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Projects.BattleSystem.Scripts;
using Projects.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine.Serialization;
using VContainer.Unity;
using VContainer;


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {    
        [SerializeField] FloorTimer_Net floorTimerNet;
        CarryPlayerSpawner _carryPlayerSpawner;
        IMapUpdater _entityGridMapSwitcher;
        HoldingBlockObserver _holdingBlockObserver;
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(
            CarryPlayerSpawner carryPlayerSpawner,
            IMapUpdater entityGridMapSwitcher,
            HoldingBlockObserver holdingBlockObserver
            )
        {
            _carryPlayerSpawner = carryPlayerSpawner;
            _entityGridMapSwitcher = entityGridMapSwitcher;
            _holdingBlockObserver = holdingBlockObserver;
        }



        async void Start()
        {
            var runner = FindObjectOfType<NetworkRunner>();
            runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner));

            
            // Start Timer
            if (Runner.IsServer)
            {
                Runner.AddSimulationBehaviour(floorTimerNet);
                floorTimerNet.StartTimer();
            }
            
            
            // Generate map
            if (Runner.IsServer)
            {
                _entityGridMapSwitcher.InitUpdateMap(MapKey.Default, 1);  // ToDo : keyやindexの計算処理を追加する
            }
            
            // IsHoldingBlockObserver
            if (Runner.IsServer)
            {
                _holdingBlockObserver.StartObserve();   
            }

            if (Runner.IsServer) _carryPlayerSpawner.RespawnAllPlayer();

            IsInitialized = true;

        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _carryPlayerSpawner.SpawnPlayer(player );
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _carryPlayerSpawner.DespawnPlayer(player);
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer) SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }


    }

}
