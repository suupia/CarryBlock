using System.Linq;
using Animations;
using Decoration;
using Fusion;
using JetBrains.Annotations;
using Main;
using UnityEngine;

#nullable  enable

namespace Boss
{
    public class Boos1Controller_Net : PoolableObject
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
        public float ActionCoolTime => _context.CurrentState.ActionCoolTime;
         IBoss1AttackSelector _actionSelector;
         IBoss1Context _context;
         IBoss1State[] _actionStates;
         IBoss1State _idleState;
         
         Transform? _targetUnit;

        // For Debug
        [SerializeField] bool showGizmos;

        // Flag for DI and PoolableObject
        bool _isInitialized;

        public void Init(IBoss1AttackSelector attackSelector)
        {
            // Init Domain
            _actionSelector = attackSelector;

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

            _context = new Boss1Context(new IdleState(_record));
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

            _actionStates = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, Runner),
                new VacuumState(_record),
                new ChargeJumpState(_record, _context)
            };
            _isInitialized = true;
        }
        

        protected override void OnInactive()
        {
            if (!_isInitialized) return;
            //　SetActive(false)された時の処理を書く
            _record.Rb.velocity = Vector3.zero;
        }

        public override void FixedUpdateNetwork()
        {
            if (!_isInitialized) return;
            if (!HasStateAuthority) return;

            _context.CurrentState.Move();

            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                // Search()を実行する
                var units = _context.CurrentState.Search();
                if (units != null && units.Any())
                {
                    // Actionを決定する
                    var actionState = _actionSelector.SelectAction(_actionStates);
                    _context.ChangeState(actionState);
                
                    // targetを決定し、必要があればtargetをセットする
                    _targetUnit = _context.CurrentState.DetermineTarget(units);
                    if(_context.CurrentState.EnemyMove is IEnemyTargetMoveExecutor targetMoveExecutor)
                        targetMoveExecutor.Target = _targetUnit;
                    if(_context.CurrentState.EnemyAction is IEnemyTargetActionExecutor targetActionExecutor)
                        targetActionExecutor.Target = _targetUnit;
                
                    // Actionを実行する
                    _context.CurrentState.StartAction();
                    
                    // Decoration
                    StartDecoration( ref DecorationDataRef);
                }
                else
                {
                    // Actionの終了処理を行う
                    _context.CurrentState.EndAction();
                    
                    // Decoration
                    EndDecoration(ref DecorationDataRef);
                
                    // Searchステートに切り替え
                    if (_context.CurrentState is IdleState) return;
                    _context.ChangeState(_idleState);
                }
            }

        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, Hp);
        }
        
        

        // 以下はデコレーション用
        void StartDecoration(ref Boss1DecorationDetector.Data data)
        {
            // For Decoration
            var attack = _context.CurrentState;
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