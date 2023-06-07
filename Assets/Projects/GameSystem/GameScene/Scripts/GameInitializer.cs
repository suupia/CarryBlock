using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Nuts.Utility.Scripts;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.BattleSystem.Boss.Scripts;
using Nuts.BattleSystem.Spawners.Scripts;
using Nuts.GameSystem.Scripts;
using UnityEngine;

namespace Nuts.GameSystem.GameScene.Scripts
{
    public class GameInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkWaveTimer _networkWaveTimer;
        NetworkPlayerSpawner _networkPlayerSpawner;
        readonly NetworkPlayerContainer _abstractNetworkPlayerContainer = new();
        
        // enemy
        SpawnerTransformContainer _enemySpawnerTransformContainer;
        readonly NetworkEnemyContainer _networkEnemyContainer = new();
        EnemySpawnersBatchExecutor _enemySpawnersBatchExecutor;
        
        // boss1
         SpawnerTransformContainer _boss1SpawnerTransformContainer;
        readonly Boss1Container _boss1Container = new();
        Boss1SpawnersBatchExecutor _boss1SpawnersBatchExecutor;

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

            // Domain
            var playerPrefabSpawner = new NetworkPlayerPrefabSpawner(Runner);
            _networkPlayerSpawner = new NetworkPlayerSpawner(Runner, playerPrefabSpawner);
            Runner.AddSimulationBehaviour(_networkWaveTimer);
            _networkWaveTimer.Init();
            
            //スポナーの位置を決定しているTransformを取得し、Controllerにわたす
            //ControllerによってTransformの数だけEnemySpawnerがインスタンス化され
            //Controllerがそれらの責任を負う
            _enemySpawnerTransformContainer = new SpawnerTransformContainer("EnemySpawnerTransform");
            _enemySpawnerTransformContainer.AddRangeByTag();
            _enemySpawnersBatchExecutor = new EnemySpawnersBatchExecutor(Runner, _enemySpawnerTransformContainer);
            
            _boss1SpawnerTransformContainer = new SpawnerTransformContainer("BossSpawnerTransform");
            _boss1SpawnerTransformContainer.AddRangeByTag();
            _boss1SpawnersBatchExecutor = new Boss1SpawnersBatchExecutor(Runner, _boss1SpawnerTransformContainer);

            Debug.Log("Please press F1 to start spawning");


            if (Runner.IsServer) _networkPlayerSpawner.RespawnAllPlayer(_abstractNetworkPlayerContainer);

            IsInitialized = true;

        }

        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (Runner.IsServer) _networkPlayerSpawner.SpawnPlayer(player, _abstractNetworkPlayerContainer);
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _networkPlayerSpawner.DespawnPlayer(player, _abstractNetworkPlayerContainer);
        }

        // Return to LobbyScene
        public void SetActiveLobbyScene()
        {
            if (Runner.IsServer) SceneTransition.TransitioningScene(Runner, SceneName.LobbyScene);
        }
        
        
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                _enemySpawnersBatchExecutor.StartSimpleSpawner(_networkEnemyContainer);
                _boss1SpawnersBatchExecutor.StartSimpleSpawner(
                    _boss1Container,
                    startSimpleSpawnerDelegate:(_, _) => new StartSimpleSpawnerRecord()
                    {
                        Index = 0,
                        Interval = 10f,
                    });
                Debug.Log("Spawn Loop was Started");
            }
            else if (Input.GetKeyDown(KeyCode.F2))
            {
                _enemySpawnersBatchExecutor.CancelSpawning();
                Debug.Log("Spawn Loop was Canceled");
            }
            else if (Input.GetKeyDown(KeyCode.F3))
            {
                _enemySpawnersBatchExecutor.StartSimpleSpawner(
                    _networkEnemyContainer,
                    startSimpleSpawnerDelegate: (i, _) => new StartSimpleSpawnerRecord()
                    {
                        Index = 0,
                        Interval = i + 2f
                    });
            }
        }
        
        [SerializeField] NetworkPrefabRef boss1Prefab; // 本来はResourceフォルダかAddressable(?)から取得する

        bool _isSetupComplete;

        enum StateUnderTest
        {
            Tackle,
            SpitOut,
            Vacuum,
            Jump,
            Random
        }


        void SpawnBoss1dads()
        {
            var boss1 = SpawnBoss1();
            var actionSelector = new RandomActionSelector(); // アクションの決定方法はランダム
            boss1.Init(actionSelector);
        }
        

        Boss1Controller_Net SpawnBoss1()
        {
            var position = gameObject.transform.position;
            var boss1Obj = Runner.Spawn(boss1Prefab, position, Quaternion.identity, PlayerRef.None);
            var boss1 = boss1Obj.GetComponent<Boss1Controller_Net>();
            return boss1;
        }


        
    }
}