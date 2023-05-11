using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss.Tests
{
    public class Boss1ControllerTestWithDecoration_Net : PoolableObject
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
            Vacuum = 2,
            ChargeJump = 3
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
            //　ファイナライザ的な処理を書く
            if (!_isInitialized) return;
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
                    _boss1.SelectAttackState(new FixedAttackSelector((int)stateUnderTest)); // テストのためにここで設定？もっといい方法ある？
                    _boss1.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
                else
                {
                    _boss1.SetSearchState();
                }
            }

            _boss1.Process();
        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            var context = new Boss1Context(new SearchPlayerState(_record));
            _boss1 = new Boss1IncludeDecorationDetector(_record, modelObject, context, Runner);
        }
    }
}