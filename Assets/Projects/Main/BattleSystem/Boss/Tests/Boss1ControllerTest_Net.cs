using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss.Tests
{
    public class Boss1ControllerTest_Net : PoolableObject
    {
        // Serialize Record
        [SerializeField] Boss1Record _record;
        [SerializeField] GameObject _modelPrefab; // ToDo: インスペクタで設定する作りはよくない ロードする作りに変える

        // INetworkStruct
        ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();

        // Networked TickerTimer
        // Cooldown Timer
        [Networked] TickTimer AttackCooldown { get; set; }

        // Stats
        [Networked] int Hp { get; set; } = 1;

        // Domain
        Boss1IncludeDecorationDetector _boss1;

        // Flag for PoolableObject
        bool _isInitialized;

        // For Tests
        enum StateUnderTest
        {
            Tackle = 0,
            SpitOut = 1,
            Vacuum = 2
        }

        [SerializeField] StateUnderTest stateUnderTest;

        public override void Spawned()
        {
            Init();
        }

        void Init()
        {
            // Init Record
            _record.Init(Runner, gameObject);

            // Instantiate the boss.
            InstantiateBoss();
            _isInitialized = true;
        }

        protected override void OnInactive()
        {
            if (!_isInitialized) return;
            //　ファイナライザ的な処理を書く
            _record.Rd.velocity = Vector3.zero;
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;


            _boss1.Move();

            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                var searchResult = _boss1.Search();
                if (searchResult.Length > 0)
                {
                    _boss1.ChooseAttackState(new FixedAttackSelector((int)stateUnderTest));
                    _boss1.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
                else
                {
                    _boss1.SetSearchState();
                }
            }
        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            var stateGenerator = new Boss1StateGenerator(Runner, _record);
            var context = new Boss1Context(stateGenerator.SearchPlayerState);
            _boss1 = new Boss1IncludeDecorationDetector(_record, modelObject, stateGenerator, context);
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