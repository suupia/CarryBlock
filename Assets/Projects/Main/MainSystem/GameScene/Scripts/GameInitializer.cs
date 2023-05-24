using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace Main
{
    public class GameInitializer : SimulationBehaviour, IPlayerJoined, IPlayerLeft
    {
        [SerializeField] NetworkWaveTimer _networkWaveTimer;
        NetworkPlayerSpawner _networkPlayerSpawner;
        readonly NetworkPlayerContainer _abstractNetworkPlayerContainer = new();
        readonly NetworkEnemyContainer _networkEnemyContainer = new();
        readonly SpawnerTransformContainer _enemySpawnerTransformContainer = new();
        EnemySpawnersBatchExecutor _enemySpawnersBatchExecutor;

        [SerializeField] string overrideSessionName;

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
            _enemySpawnerTransformContainer.AddRangeByTag();
            _enemySpawnersBatchExecutor = new EnemySpawnersBatchExecutor(Runner, _enemySpawnerTransformContainer);

            Debug.Log("Please press F1 to start spawning");


            if (Runner.IsServer) _networkPlayerSpawner.RespawnAllPlayer(_abstractNetworkPlayerContainer);


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
    }
}