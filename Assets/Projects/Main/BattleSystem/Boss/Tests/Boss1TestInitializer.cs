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
            Tackle = 0,
            Jump = 1,
            SpitOut = 2,
            Vacuum = 3
        }

        async void Start()
        {
            var runnerManager = FindObjectOfType<NetworkRunnerManager>();
            // Runner.StartGame() if it has not been run.
            await runnerManager.AttemptStartScene("GameSceneTestRoom");
            runnerManager.Runner.AddSimulationBehaviour(this); // Register this class with the runner
            await UniTask.WaitUntil(() => Runner.SceneManager.IsReady(Runner),
                cancellationToken: new CancellationToken());

            _isSetupComplete = true;
        }

        void Update()
        {
            if (!_isSetupComplete) return;

            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                var boss1 = SpawnBoss1();
                boss1.Init(new RandomAttackSelector());
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                var boss1 = SpawnBoss1();
                boss1.Init(CreateTestAttackSelector(1));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                var boss1 = SpawnBoss1();
                boss1.Init(CreateTestAttackSelector(2));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                var boss1 = SpawnBoss1();
                boss1.Init(CreateTestAttackSelector(3));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                var boss1 = SpawnBoss1();
                boss1.Init(CreateTestAttackSelector(4));
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