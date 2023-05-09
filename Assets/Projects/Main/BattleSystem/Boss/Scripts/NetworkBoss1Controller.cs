using Animations;
using Decoration;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    public class NetworkBoss1Controller : PoolableObject
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

        public override void Spawned()
        {
            Debug.Log("Spawned");
            Init();
        }

        void Init()
        {
            // Init record.
            _record = new Boss1Record(Runner, gameObject);

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
                    _boss1.ChooseAttackState();
                    _boss1.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
            }

        }

        void InstantiateBoss()
        {
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            var stateGenerator = new Boss1StateGenerator(Runner, _record);
            var context = new Boss1Context(stateGenerator.LostState);
            _boss1 = new Boss1IncludeDecorationDetector(modelObject, stateGenerator, context);
        }
    }

    public class Boss1IncludeDecorationDetector
    {
        // Domain
        readonly Boss1StateGenerator _stateGenerator;
        readonly IBoss1Context _context;

        // Decoration
        Boss1DecorationDetector _decorationDetector;

        public Boss1IncludeDecorationDetector(GameObject modelObject, Boss1StateGenerator stateGenerator,
            IBoss1Context context)
        {
            _context = context;
            _stateGenerator = stateGenerator;
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));
        }

        public void ChooseAttackState()
        {
            // ToDo: ここで攻撃ステートを決める
            // とりあえずTackleにする
            _context.ChangeState(_stateGenerator.TackleState);
        }

        public void Move()
        {
            _context.CurrentState.Move();
        }

        public Collider[] Search()
        {
            return _context.CurrentState.Search();
        }

        public void Attack()
        {
            _context.CurrentState.Attack();
        }
    }

}