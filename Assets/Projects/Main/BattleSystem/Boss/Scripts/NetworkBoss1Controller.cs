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
        [SerializeField] Transform modelParent;

        // Networked Properties
        [Networked] ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();
        [Networked] TickTimer AttackCooldown { get; set; }

        // Tmp
        [Networked] int Hp { get; set; } = 1;

        // Decoration Detector
        Boss1DecorationDetector _decorationDetector;

        // Domain
        Boss1IncludeDecorationDetector _boss1;
        IBoss1AttackSelector _attackSelector;

        // Flag for PoolableObject
        bool _isInitialized;

        public void Init(IBoss1AttackSelector attackSelector)
        {
            // Init Record
            _record.Init(Runner, gameObject);

            // Init Domain
            _attackSelector = attackSelector;

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
            if (!_isInitialized) return;
            if (!HasStateAuthority) return;
            
            _boss1.Move();

            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                var searchResult = _boss1.Search();
                if (searchResult.Length > 0)
                {
                    _boss1.SelectAttackState(_attackSelector, ref DecorationDataRef);
                    _boss1.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
                else
                {
                    _boss1.SetSearchState(ref DecorationDataRef);
                }
            }

            _boss1.Process();

        }

        void InstantiateBoss()
        {
            // Instantiate
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform, modelParent);

            // Init Decoration
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

            var context = new Boss1Context(new SearchPlayerState(_record));
            _boss1 = new Boss1IncludeDecorationDetector(_record, context, _decorationDetector, Runner);
        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, Hp);
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
        readonly IBoss1State[] _attacks;

        // Decoration
        readonly Boss1DecorationDetector _decorationDetector;

        public Boss1IncludeDecorationDetector(Boss1Record record, IBoss1Context context,
            Boss1DecorationDetector decorationDetector,
            NetworkRunner runner)
        {
            _record = record;
            _context = context;
            _decorationDetector = decorationDetector;
            _runner = runner;

            _attacks = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, _runner),
                new VacuumState(_record),
                new ChargeJumpState(_record)
            };
        }

        public void SelectAttackState(IBoss1AttackSelector attackSelector, ref Boss1DecorationDetector.Data data)
        {
            var attack = attackSelector.SelectAttack(_attacks);

            // Decoration
            StartDecoration(attack, ref data);

            // ChangeState
            _context.ChangeState(attack);
        }

        public void SetSearchState(ref Boss1DecorationDetector.Data data)
        {
            // Decoration
            EndDecoration(ref data); // ここにあるのはちょっと変かも

            // ChangeState
            if (_context.CurrentState is SearchPlayerState) return;
            _context.ChangeState(new SearchPlayerState(_record));
        }

        public void Move(Vector3 input = default)
        {
            var state = _context.CurrentState;
            if (state is
                {
                    EnemyMove : ITargetMove move, EnemyAttack: ITargetAttack attack
                }) // ToDo: ここにあるのは変なのでうまく切り出す
            {
                move.Target = attack.Target;
                Debug.Log("Targetをセット！ move.Target = " + move.Target);
            }

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

        public void Process()
        {
            _context.CurrentState.Process(_context);
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