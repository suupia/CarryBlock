using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using UnityEngine;
using Main;

# nullable enable

namespace Boss
{
    // 以下はNetworkBehaviourのためのインターフェース
    public interface IEnemyMove
    {
        void Move(); 
    }
    
    public interface IEnemyAction
    {
        float ActionCoolTime { get; }
        void Action();
    }

    public interface  IEnemy : IEnemyMove, IEnemyAction
    {

    }

    // 以下はドメインのEnemyクラスのためのインターフェース

    public interface IEnemyMoveExecutor
    {
        void Move();
    }
    public interface IEnemyTargetMoveExecutor : IEnemyMoveExecutor
    {
        Transform Target { get; set; }
    }

    public interface IEnemyActionExecutor
    {
        float ActionCoolTime { get; }　//ToDo: 関数でもいいかも
        void StartAction();
        void EndAction();
    }

    public interface IEnemyTargetActionExecutor : IEnemyActionExecutor
    {
        Transform? Target { get; set; }
    }

    public interface IEnemySearchExecutor
    {
        Transform[]?  Search();
        Transform? DetermineTarget(IEnumerable<Transform> targetUnits); // Moveのターゲットなどに使われる
        // もし、Action側で複数のTransformを受け取る必要がある場合は、IEnemyTargetsActionExecutorとDetermineTargetsを作成する

    }
    
    public class ExampleEnemy : IEnemy
    {
        public float ActionCoolTime => _action.ActionCoolTime;
        readonly IEnemyMoveExecutor _move;
        readonly IEnemyActionExecutor _action;
        readonly IEnemySearchExecutor _search;

        Transform? _targetUnit;

        public ExampleEnemy()
        {
            // いろいろ受け取って初期化
        }

        // NetworkFixedUpdateで呼ばれるためパフォーマンスに注意
        public void Move()
        {
            _move.Move();
        }

        // このメソッドを呼んだ後にActionCoolTimeを回す
        public void Action()
        {
            // ここでSearchを使う
            var units = _search.Search();
            if (units != null && units.Any())
            {
                _targetUnit = _search.DetermineTarget(units);
                if(_move is IEnemyTargetMoveExecutor targetMoveExecutor)
                    targetMoveExecutor.Target = _targetUnit;
                if(_action is IEnemyTargetActionExecutor targetActionExecutor)
                    targetActionExecutor.Target = _targetUnit;
                _action.StartAction();
            }
        }
        
    }
    
    // 具体的なクラスの例
    // これをNetworkBehaviourのフィールドとし、Initで受け取るようにしたい
    public class ExampleStateEnemy : IEnemy
    {
        public float ActionCoolTime => _context.CurrentState.ActionCoolTime;
        readonly Boss1Record _record;
        readonly IBoss1AttackSelector _actionSelector;
        readonly IBoss1State[] _actionStates;
        readonly IBoss1State _idleState;
        readonly IBoss1Context _context;
        

        Transform? _targetUnit;

        public ExampleStateEnemy(NetworkRunner runner, Boss1Record record, IBoss1AttackSelector actionSelector)
        {
            _record = record;
            _actionSelector = actionSelector;

            _context = new Boss1Context(_idleState);
            _idleState = new WanderState(record);
            var jumpState = new JumpState(_record,_idleState, _context);
            _actionStates = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, runner),
                new VacuumState(_record),
                new ChargeJumpState(_record, jumpState, _context) // 内部でJumpStateに遷移するため、contextに依存している
            };
        }
    
        public void Move()
        {
            _context.CurrentState.Move();
        }
        
        // このメソッドを呼んだ後にAttackCoolTimeを回す
        public void Action()
        {
            // Search()を実行する
            var units = _context.CurrentState.Search();
            if (units.Any())
            {
                // Actionを決定する
                var actionState = _actionSelector.SelectAction(_actionStates);
                _context.ChangeState(actionState);
                
                // targetを決定し、必要があればtargetをセットする
                _targetUnit = _context.CurrentState.DetermineTarget(units);
                 if(_context.CurrentState is IEnemyTargetMoveExecutor targetMoveExecutor)
                     targetMoveExecutor.Target = _targetUnit;
                 if(_context.CurrentState is IEnemyTargetActionExecutor targetActionExecutor)
                     targetActionExecutor.Target = _targetUnit;
                
                // Actionを実行する
                _context.CurrentState.StartAction();
            }
            else
            {
                // Actionの終了処理を行う
                _context.CurrentState.EndAction();
                
                // Searchステートに切り替え
                if (_context.CurrentState is WanderState) return;
                _context.ChangeState(_idleState);
            }
        }
    }
}

