using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using UnityEngine;

namespace Boss.Tests
{
    public class Boss1TestInitializer : NetworkBehaviour
    {
        [SerializeField] NetworkPrefabRef boss1Prefab; // 本来はResourceフォルダかAddressable(?)から取得する

        bool _isSetupComplete;

        enum StateUnderTest
        {
            Tackle,
            Jump,
            SpitOut,
            Vacuum,
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
                        boss1.Init(new RandomAttackSelector());
                    else
                        // Fixed
                        boss1.Init(CreateTestAttackSelector(i));
                }
        }
        

        NetworkBoss1Controller SpawnBoss1()
        {
            var position = gameObject.transform.position;
            var boss1Obj = Runner.Spawn(boss1Prefab, position, Quaternion.identity, PlayerRef.None);
            var boss1 = boss1Obj.GetComponent<NetworkBoss1Controller>();
            return boss1;
        }

        IBoss1AttackSelector CreateTestAttackSelector(int index)
        {
            return new FixedAttackSelector(index);
        }


    }

    public class FixedAttackSelector : IBoss1AttackSelector
    {
        readonly int _attackIndex;

        public FixedAttackSelector(int attackIndex)
        {
            _attackIndex = attackIndex;
        }

        public IBoss1State SelectAttack(params IBoss1State[] attacks)
        {
            if (0 <= _attackIndex && _attackIndex < attacks.Length) return attacks[_attackIndex];

            Debug.LogError($"_attackIndex({_attackIndex}) is out of range.");
            return attacks[0];
        }
    }
    

}