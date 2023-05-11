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

        // Networked Properties
        [Networked] ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();
        [Networked] TickTimer AttackCooldown { get; set; }

        // Decoration Detector
        Boss1DecorationDetector _decorationDetector;

        // Domain
        Boss1IncludeDecorationDetector _boss1;

        // Flag for PoolableObject
        bool _isInitialized;

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
                    _boss1.SelectAttackState(new RandomAttackSelector(), ref DecorationDataRef);
                    _boss1.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
                else
                {
                    _boss1.SetSearchState(ref DecorationDataRef);
                }
            }

        }

        void InstantiateBoss()
        {
            // Instantiate
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform);

            // Init Decoration
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

            var context = new Boss1Context(new SearchPlayerState(_record));
            _boss1 = new Boss1IncludeDecorationDetector(_record, modelObject, context, _decorationDetector, Runner);
        }
    }

    public interface IBoss1AttackSelector
    {
        IBoss1State SelectAttack(params IBoss1State[] attacks);
    }

    public class RandomAttackSelector : IBoss1AttackSelector
    {
        public IBoss1State SelectAttack(params IBoss1State[] attacks)
        {
            // 0からattacks.Length-1までのランダムな整数を取得
            var randomIndex = Random.Range(0, attacks.Length);
            return attacks[randomIndex];
        }
    }

    public class Boss1IncludeDecorationDetector
    {
        // NetworkRunner
        readonly NetworkRunner _runner; // SpitOutStateのためだけにある

        // Domain
        readonly Boss1Record _record; //ToDo: targetbufferを取得するためだけにあるので、いずれ消す
        readonly IBoss1Context _context;

        // Decoration
        readonly Boss1DecorationDetector _decorationDetector;

        public Boss1IncludeDecorationDetector(Boss1Record record, GameObject modelObject, IBoss1Context context,
            Boss1DecorationDetector decorationDetector,
            NetworkRunner runner)
        {
            _record = record;
            _context = context;
            _decorationDetector = decorationDetector;
            _runner = runner;
        }

        public void SelectAttackState(IBoss1AttackSelector attackSelector, ref Boss1DecorationDetector.Data data)
        {
            var attacks = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, _runner),
                new VacuumState(_record),
                new ChargeJumpState(_record)
            };
            var attack = attackSelector.SelectAttack(attacks);

            // Decoration
            StartDecoration(attack, ref data);

            // ChangeState
            _context.ChangeState(attack);
        }

        public void SetSearchState(ref Boss1DecorationDetector.Data data)
        {
            // Decoration
            EndDecoration(ref data);

            // ChangeState
            _context.ChangeState(new SearchPlayerState(_record));
        }

        public void Move(Vector3 input = default)
        {
            var state = _context.CurrentState;
            if (state is { EnemyMove : ITargetMove move, EnemyAttack: ITargetAttack attack })
                move.Target = attack.Target;

            state.Move(input);

        }

        public Collider[] Search()
        {
            var searchResult = _context.CurrentState.Search();
            _record.TargetBuffer.Clear();
            _record.TargetBuffer.UnionWith(searchResult.Map(c => c.transform));
            return searchResult;
        }

        public void Attack()
        {
            _context.CurrentState.Attack();
        }

        void StartDecoration(IBoss1State attack, ref Boss1DecorationDetector.Data data)
        {
            // For Decoration
            if (attack is TackleState)
            {
                _decorationDetector.OnStartTackle(ref data);
            }
            else if (attack is SpitOutState)
            {
                // これは用意されていない
                // decorationDetector.OnStartSpitOut(ref data);
            }
            else if (attack is VacuumState)
            {
                _decorationDetector.OnStartVacuum(ref data);
            }
            else if (attack is ChargeJumpState)
            {
                _decorationDetector.OnStartJump(ref data);
            }
        }

        void EndDecoration(ref Boss1DecorationDetector.Data data)
        {
            // For Decoration
            var attack = _context.CurrentState;
            if (attack is TackleState)
            {
                _decorationDetector.OnEndTackle(ref data);
            }
            else if (attack is SpitOutState)
            {
                // これは用意されていない
                // decorationDetector.OnEndSpitOut(ref data);
            }
            else if (attack is VacuumState)
            {
                _decorationDetector.OnEndVacuum(ref data);
            }
            else if (attack is ChargeJumpState)
            {
                _decorationDetector.OnEndJump(ref data);
            }
        }
    }

}