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
        IBoss1Context _context;
        IBoss1State[] _attackStates;
        IBoss1AttackSelector _attackSelector;

        // For Debug
        [SerializeField] bool showGizmos;

        // Flag for DI and PoolableObject
        bool _isInitialized;

        public void Init(IBoss1AttackSelector attackSelector)
        {
            // Init Domain
            _attackSelector = attackSelector;

            RPC_LocalInit();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_LocalInit()
        {
            // Init Record
            _record.Init(Runner, gameObject);
            
            // Instantiate
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, gameObject.transform, modelParent);

            _context = new Boss1Context(new SearchPlayerState(_record));
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

            _attackStates = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, Runner),
                new VacuumState(_record),
                new ChargeJumpState(_record)
            };
            _isInitialized = true;
        }
        

        protected override void OnInactive()
        {
            if (!_isInitialized) return;
            //　SetActive(false)された時の処理を書く
            _record.Rd.velocity = Vector3.zero;
        }

        public override void FixedUpdateNetwork()
        {
            if (!_isInitialized) return;
            if (!HasStateAuthority) return;

            Move();

            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                var searchResult = Search();
                if (searchResult.Length > 0)
                {
                    SelectAttackState(_attackSelector, ref DecorationDataRef);
                    _context.CurrentState.Attack();
                    AttackCooldown = TickTimer.CreateFromSeconds(Runner, _record.DefaultAttackCoolTime);
                }
                else
                {
                    SetSearchState(ref DecorationDataRef);
                }
            }

            _context.CurrentState.Process(_context);

        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, Hp);
        }
        
        // 以下privateメソッド

        void SelectAttackState(IBoss1AttackSelector attackSelector, ref Boss1DecorationDetector.Data data)
        {
            var attack = attackSelector.SelectAttack(_attackStates);

            // Decoration
            StartDecoration(attack, ref data);

            // ChangeState
            _context.ChangeState(attack);
        }

        void SetSearchState(ref Boss1DecorationDetector.Data data)
        {
            // Decoration
            EndDecoration(ref data); // ここにあるのはちょっと変かも

            // ChangeState
            if (_context.CurrentState is SearchPlayerState) return;
            _context.ChangeState(new SearchPlayerState(_record));
        }

        void Move(Vector3 input = default)
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
        
        Collider[] Search()
        {
            var searchResult = _context.CurrentState.Search();
            _record.TargetBuffer.Clear();
            _record.TargetBuffer.UnionWith(searchResult.Map(c => c.transform));
            return searchResult;
        }

        // 以下はデコレーション用
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

        // 以下はデバッグ用
        void OnDrawGizmos()
        {
            if (!showGizmos) return;
            //サーチ範囲を表示
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _record.SearchRadius);
        }

    }

}