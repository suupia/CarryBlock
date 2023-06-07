#nullable  enable

using System;
using System.Linq;
using Fusion;
using Nuts.NetworkUtility.ObjectPool.Scripts;
using Nuts.BattleSystem.Decoration.Scripts;
using Nuts.Projects.BattleSystem.Decoration.Scripts;
using Nuts.BattleSystem.Enemy.Scripts.Player.Attack;
using UnityEngine;
using  Nuts.BattleSystem.Enemy.Monster.Interfaces;

namespace Nuts.BattleSystem.Boss.Scripts
{
    public class Monster1Controller_Net : PoolableObject, IEnemyOnAttacked
    {
        // Serialize Record
        [SerializeField] Monster1Record _record;
        [SerializeField] GameObject _modelPrefab; // ToDo: インスペクタで設定する作りはよくない ロードする作りに変える
        [SerializeField] Transform modelParent;

        public Transform InterpolationTransform => modelParent;

        // Networked Properties
        [Networked] ref Boss1DecorationDetector.Data DecorationDataRef => ref MakeRef<Boss1DecorationDetector.Data>();
        [Networked] TickTimer AttackCooldown { get; set; }

        // Tmp
        [Networked] public int Hp { get; set; }

        // Decoration Detector
        Boss1DecorationDetector _decorationDetector;

        // Domain
        IBoss1ActionSelector _actionSelector;
        IMonster1State _idleState;
        IMonster1State _wanderState;
        IMonster1State _jumpState;
        IMonster1State[] _actionStates;
        IMonster1Context _context;
        
        IMonster1State _beforeState;

         Transform? _targetUnit;
         
         public Action OnDespawn = () => { }; // EnemyContainerから削除する処理が入る

        // For Debug
        [SerializeField] bool showGizmos;

        // Flag for DI and PoolableObject
        bool _isInitialized;

        // Initializerから注入したいオブジェクトを受け取る
        // よっぽどのことがない限り、初期化の処理はRPCの方に書いた方が安全
        public void Init(IBoss1ActionSelector actionSelector)
        {
            // Init Host Domain
            _actionSelector = actionSelector;

            Hp = 10; // 一時的にここに書いておく

            RPC_LocalInit();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_LocalInit()
        {
            // Init Record
            _record.Init(Runner, gameObject);
            
            // Init Host Domain
            // newの順番に注意！
            _wanderState = new WanderState(_record);
            _idleState = new IdleState(_record);
            _jumpState = new JumpState(_record);
            _context = new Monster1Context(_idleState);
            _actionStates = new IMonster1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, Runner),
                new VacuumState(_record),
                new ChargeJumpState(_record)
            };

            
            // Instantiate
            var prefab = _modelPrefab;
            var modelObject = Instantiate(prefab, modelParent);
           
            _decorationDetector = new Boss1DecorationDetector(new Boss1AnimatorSetter(modelObject));

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
            
            if (AttackCooldown.ExpiredOrNotRunning(Runner))
            {
                // デコレーションの終了処理
                EndDecoration(ref DecorationDataRef);

                // 次のDomainのStateを決定する (Actionを終了する前に行うことに注意)
                Debug.Log($"before state = {_context.CurrentState}");
                DecideNextState();
                Debug.Log($"after state = {_context.CurrentState}");

                
                // DomainのActionの終了処理を行う
                Debug.Log($"is action completed = {_context.CurrentState.IsActionCompleted}");
                if (_context.CurrentState.IsActionCompleted || _beforeState != _context.CurrentState)
                {
                    _context.CurrentState.EndAction();
                }
                
                
                // DomainのActionの開始処理
                _context.CurrentState.StartAction();
                AttackCooldown = TickTimer.CreateFromSeconds(Runner, _context.CurrentState.ActionCoolTime);
                
                // デコレーションの開始処理
                StartDecoration( ref DecorationDataRef);
                
            }
            
            _context.CurrentState.Move();

        }

        public override void Render()
        {
            _decorationDetector.OnRendered(DecorationDataRef, Hp);
        }

        public void OnAttacked(int damage)
        {
            if(!HasStateAuthority) return;
            Hp -= damage;
            if(Hp <= 0)OnDefeated();
        }

        void DecideNextState()
        {
            _beforeState = _context.CurrentState;
            if (_beforeState is IdleState)
            {
                if (_beforeState.IsActionCompleted)
                    _context.ChangeState(_wanderState);
            }
            else if (_beforeState is ChargeJumpState)
            {
                if (_beforeState.IsActionCompleted)
                    _context.ChangeState(_jumpState);
            }
            else
            {
                // Searchを実行する
                var units = _beforeState.Search();
                if (units != null && units.Any())
                {
                    // 次のActionを決定する
                    var actionState = _actionSelector.SelectAction(_actionStates);
                    _context.ChangeState(actionState);

                    // targetを決定し、必要があればtargetをセットする
                    _targetUnit = _context.CurrentState.DetermineTarget(units);
                    if(_context.CurrentState.EnemyMove is IEnemyTargetMove targetMoveExecutor)
                        targetMoveExecutor.Target = _targetUnit;
                    if(_context.CurrentState.EnemyAction is IEnemyTargetAction targetActionExecutor)
                        targetActionExecutor.Target = _targetUnit;

                }
                else
                {
                    if (_context.CurrentState is WanderState)
                    {
                        // WanderStateの場合は何もしない
                    }
                    else
                    {
                        // IdleStateに戻る
                        _context.ChangeState(_idleState);
                        AttackCooldown = TickTimer.CreateFromSeconds(Runner, _context.CurrentState.ActionCoolTime);
                    }
                }
            }
        }

        void OnDefeated()
        {
            // ボスを倒したときのドロップアイテムを出す　c.f. EnemyController
            OnDespawn();
            Runner.Despawn(Object);
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