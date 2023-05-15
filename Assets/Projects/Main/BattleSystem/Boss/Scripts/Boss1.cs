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

    // 以下はUnit側のインターフェース
    // ToDo:後で移動

    // public interface IUnitOnAttacked
    // {
    //     NetworkPlayerStruct OnAttacked(ref NetworkPlayerStruct networkPlayerStruct, int damage);
    // }
    //
    // public interface IUnitOnTargeted : IUnitOnAttacked
    // {
    //     Transform targetTransform { get; }
    // }

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
        public float ActionCoolTime => _record.DefaultAttackCoolTime; // Attackごとに変えるかも → StateでIEnemyAttackを実装するようにする
        readonly Boss1Record _record;
        readonly IBoss1AttackSelector _actionSelector;
        readonly IBoss1State[] _actionStates;
        readonly IBoss1State _searchState;
        readonly IBoss1Context _context;
        
        Transform? _targetUnit;

        public ExampleStateEnemy(NetworkRunner runner, Boss1Record record, IBoss1AttackSelector actionSelector)
        {
            _record = record;
            _actionSelector = actionSelector;
            
            _searchState = new SearchPlayerState(record);
            _actionStates = new IBoss1State[]
            {
                new TackleState(_record),
                new SpitOutState(_record, runner),
                new VacuumState(_record),
                new ChargeJumpState(_record)
            };

            _context = new Boss1Context(_searchState);
        }
    
        public void Move()
        {
            _context.CurrentState.Move();
        }
        
        // このメソッドを呼んだ後にAttackCoolTimeを回す
        public void Action()
        {
            // ここで絶対にSearchを使う
            var units = _context.CurrentState.Search();
            if (units.Any())
            {
                // Actionを決定する
                //var actionState = _actionSelector.SelectAction(_actionStates);
                
                // targetを決定し、必要があればtargetをセットする
                //_targetUnit = _context.CurrentState.DetermineTarget(units);
                // if(_context.CurrentState is IEnemyTargetMoveExecutor targetMoveExecutor)
                //     targetMoveExecutor.Target = _targetUnit;
                // if(_context.CurrentState is IEnemyTargetActionExecutor targetActionExecutor)
                //     targetActionExecutor.Target = _targetUnit;
                
                // Actionを実行する
                //_context.CurrentState.StartAction();
            }
            else
            {
                // Actionの終了処理を行う
                //_context.CurrentState.EndAction();
                
                // Searchステートに切り替え
                if (_context.CurrentState is SearchPlayerState) return;
                _context.ChangeState(_searchState);
            }
        }
    }
}

