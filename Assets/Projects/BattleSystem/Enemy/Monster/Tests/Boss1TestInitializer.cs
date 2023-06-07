using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Nuts.NetworkUtility.NetworkRunnerManager.Scripts;
using Nuts.BattleSystem.Boss.Scripts;
using Nuts.BattleSystem.Enemy.Monster.Interfaces;
using UnityEngine;

namespace Nuts.BattleSystem.BattleSystem.Boss.Tests
{
    public class Boss1TestInitializer : NetworkBehaviour
    {
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
            await runnerManager.AttemptStartScene("Boss1TestInitializer");
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
                cancellationToken: new CancellationToken());

            _isSetupComplete = true;
            Debug.Log("Boss1TestInitializer is ready.");
        }
        
        void OnGUI()
        {
            var stateEnums = Enum.GetNames(typeof(StateUnderTest));
            for (var i = 0; i < stateEnums.Length; i++)
                if (GUI.Button(new Rect(10, 10 + i * 30, 120, 20), stateEnums[i]))
                {
                    if (!_isSetupComplete)
                    {
                        Debug.Log("NetworkRunnerManager is not ready yet.");
                        return;
                    }

                    var boss1 = SpawnBoss1();
                    if (i == stateEnums.Length - 1)
                        // Random
                        boss1.Init(new RandomActionSelector());
                    else
                        // Fixed
                        boss1.Init(CreateTestAttackSelector(i));
                }
        }
        

        Boss1Controller_Net SpawnBoss1()
        {
            var position = gameObject.transform.position;
            var boss1Obj = Runner.Spawn(boss1Prefab, position, Quaternion.identity, PlayerRef.None);
            var boss1 = boss1Obj.GetComponent<Boss1Controller_Net>();
            return boss1;
        }

        IBoss1ActionSelector CreateTestAttackSelector(int index)
        {
            return new FixedActionSelector(index);
        }


    }


    

}