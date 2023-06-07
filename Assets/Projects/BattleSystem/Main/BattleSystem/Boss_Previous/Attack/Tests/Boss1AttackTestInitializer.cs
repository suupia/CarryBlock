using System;
using System.Threading;
using Boss;
using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using NetworkUtility.NetworkRunnerManager;
using Nuts.BattleSystem.Boss;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Boss.Tests;
using Nuts.Projects.BattleSystem.Main.BattleSystem.Spawners;
using UnityEngine;

namespace Nuts.Projects.BattleSystem.Main.BattleSystem.Boss_Previous.Attack.Tests
{
    public class Boss1AttackTestInitializer : NetworkBehaviour, IPlayerJoined, IPlayerLeft
    {
        readonly NetworkPlayerContainer _abstractNetworkPlayerContainer = new();
        readonly NetworkEnemyContainer _networkEnemyContainer = new();
        EnemySpawner _enemySpawner;
        NetworkPlayerSpawner _networkPlayerSpawner;
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

        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            // Runner.StartGame() if it has not been run.
            await runnerManager.AttemptStartScene("Boss1AttackTestInitializer");
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
                cancellationToken: new CancellationToken());
            
            
            // Domain
            var playerPrefabSpawner = new NetworkPlayerPrefabSpawner(Runner);
            _networkPlayerSpawner = new NetworkPlayerSpawner(Runner, playerPrefabSpawner);
            _enemySpawner = new EnemySpawner(Runner);


            if (Runner.IsServer) _networkPlayerSpawner.RespawnAllPlayer(_abstractNetworkPlayerContainer);

            if (Runner.IsServer)
            {
                _networkEnemyContainer.MaxEnemyCount = 5;
                var _ = _enemySpawner.StartSimpleSpawner(0, 5f, _networkEnemyContainer);
            }

            _isSetupComplete = true;
            Debug.Log("Boss1AttackTestInitializer is ready.");
        }
        
        void OnGUI()
        {
            var stateEnums = Enum.GetNames(typeof(StateUnderTest));
            var buttonWidth = 120;
            var buttonHeight = 20;
            for (var i = 0; i < stateEnums.Length; i++)
                if (GUI.Button(new Rect(Screen.width - 10 -buttonWidth , 10 + i * 30, buttonWidth, buttonHeight), stateEnums[i]))
                {
                    if (!_isSetupComplete)
                    {
                        Debug.Log("NetworkRunnerManager is not ready yet.");
                        return;
                    }

                    var boss1 = SpawnBoss1();
                    if (i == stateEnums.Length - 1)
                        // Random
                        boss1.Init(new RandomAttackSelector());
                    else
                        // Fixed
                        boss1.Init(new FixedAttackSelector(i));
                }
        }
        

        Boss1Controller_Net SpawnBoss1()
        {
            var position = gameObject.transform.position;
            var boss1Obj = Runner.Spawn(boss1Prefab, position, Quaternion.identity, PlayerRef.None);
            var boss1 = boss1Obj.GetComponent<Boss1Controller_Net>();
            return boss1;
        }
        void IPlayerJoined.PlayerJoined(PlayerRef player)
        {
            if (_networkPlayerSpawner == null) return;
            if (Runner.IsServer) _networkPlayerSpawner.SpawnPlayer(player, _abstractNetworkPlayerContainer);
            // Todo: RunnerがSetActiveシーンでシーンの切り替えをする時に対応するシーンマネジャーのUniTaskのキャンセルトークンを呼びたい
        }


        void IPlayerLeft.PlayerLeft(PlayerRef player)
        {
            if (Runner.IsServer) _networkPlayerSpawner.DespawnPlayer(player, _abstractNetworkPlayerContainer);
        }

        

    }


}