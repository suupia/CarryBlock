using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Entity.Scripts;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using UnityEngine.Serialization;
using VContainer.Unity;
using VContainer;


namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {    
        [SerializeField] FloorTimer_Net floorTimerNet;
        CarryPlayerSpawner _carryPlayerSpawner;
        
        TilePresenterBuilder _tilePresenterBuilder;
        public bool IsInitialized { get; private set; }
        
        [Inject]
        public void Construct(CarryPlayerSpawner carryPlayerSpawner, TilePresenterBuilder tilePresenterBuilder)
        {
            _carryPlayerSpawner = carryPlayerSpawner;
            _tilePresenterBuilder = tilePresenterBuilder;
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
                var vContainer = FindObjectOfType<LifetimeScope>().Container;
                var entityGridMapSwitcher = vContainer.Resolve<EntityGridMapSwitcher>();
                var tilePresentContainer = new TilePresenterAttacher();
                var tilePresenters = _tilePresenterBuilder.Build(entityGridMapSwitcher.GetMap());
                tilePresentContainer.SetTilePresenters(tilePresenters);
                // entityGridMapSwitcher.RegisterTilePresenter(Runner, tilePresenterRegister);
                entityGridMapSwitcher.RegisterTilePresenterContainer(tilePresentContainer);
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
