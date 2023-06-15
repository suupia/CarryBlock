using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Carry.CarrySystem.Map.Scripts;
using Carry.CarrySystem.Player.Scripts;
using UnityEngine;
using Fusion;
using Carry.CarrySystem.Spawners;
using Cysharp.Threading.Tasks;
using Nuts.BattleSystem.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts; 



namespace Carry.CarrySystem.CarryScene.Scripts
{
    public class CarryInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {    
        // [SerializeField] NetworkWaveTimer _networkWaveTimer;
        AbstractNetworkPlayerSpawner<CarryPlayerController_Net> _carryPlayerSpawner;
        readonly CarryPlayerContainer _carryPlayerContainer = new();

        [SerializeField] string overrideSessionName;
        
        public bool IsInitialized { get; private set; }

        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            // Runner.StartGame() if it has not been run.
            await runnerManager.AttemptStartScene(overrideSessionName);
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
                cancellationToken: new CancellationToken());
            
            // Spawn player
            var playerPrefabSpawner = new CarryPlayerPrefabSpawner(Runner);
            _carryPlayerSpawner = new CarryPlayerSpawner(Runner, playerPrefabSpawner);
            
            // Generate map
            var mapGenerator = new MapGenerator(Runner);

            if (Runner.IsServer) _carryPlayerSpawner.RespawnAllPlayer(_carryPlayerContainer);

            IsInitialized = true;

        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _carryPlayerSpawner.SpawnPlayer(player, _carryPlayerContainer);
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _carryPlayerSpawner.DespawnPlayer(player, _carryPlayerContainer);
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer) SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }
        
    }

}
