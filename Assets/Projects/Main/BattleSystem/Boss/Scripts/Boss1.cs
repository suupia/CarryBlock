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
    
    public interface IEnemyAttack
    {
        float AttackCoolTime { get; }
        void Attack();
    }

    public interface  IEnemy : IEnemyMove, IEnemyAttack
    {

    }

    // 以下はドメインのEnemyクラスのためのインターフェース

    public interface IEnemyMoveExecutor : IEnemyMove
    {
        void Move(IUnitOnTargeted? target = default);
    }
    public interface IEnemySearchExecutor
    {
        IUnitOnTargeted[]  Search();
    }

    public interface IEnemyAttackExecutor
    {
        float AttackCoolTime { get; }　//ToDo: 関数でもいいかも
        IUnitOnTargeted DetermineTarget(IEnumerable<IUnitOnTargeted> targetUnits); // Moveのターゲットなどに使われる
        void Attack(IEnumerable<IUnitOnAttacked> targetUnits);
    }

    // 以下はUnit側のインターフェース
    // ToDo:後で移動

    public interface IUnitOnAttacked
    {
        NetworkPlayerStruct OnAttacked(ref NetworkPlayerStruct networkPlayerStruct, int damage);
    }

    public interface IUnitOnTargeted : IUnitOnAttacked
    {
        Vector3 Position { get; }
    }

    public class ExampleEnemy : IEnemy
    {
        public float AttackCoolTime => _attack.AttackCoolTime;
        readonly IEnemyMoveExecutor _move;
        readonly IEnemySearchExecutor _search;
        readonly IEnemyAttackExecutor _attack;

        IUnitOnTargeted? _targetUnit;

        public ExampleEnemy()
        {
            // いろいろ受け取って初期化
        }

        // NetworkFixedUpdateで呼ばれるためパフォーマンスに注意
        public void Move()
        {
            _move.Move(_targetUnit);
        }

        // このメソッドを呼んだ後にAttackCoolTimeを回す
        public void Attack()
        {
            // ここで絶対にSearchを使う
            var units = _search.Search();
            if (units.Any())
            {
                _targetUnit = _attack.DetermineTarget(units);
                _attack.Attack(units);
            }
        }
        
    }
    
    // 具体的なクラスの例
    // これをNetworkBehaviourのフィールドとし、Initで受け取るようにしたい
    // public class Boss1 : IEnemy
    // {
    //     public float AttackCoolTime => _record.DefaultAttackCoolTime; // Attackごとに変えるかも → StateでIEnemyAttackを実装するようにする
    //     readonly Boss1Record _record;
    //     readonly IBoss1AttackSelector _attackSelector;
    //     readonly IBoss1Context _context;
    //     readonly IBoss1State[] _attackStates;
    //
    //     public Boss1(NetworkRunner runner, Boss1Record record, IBoss1AttackSelector attackSelector)
    //     {
    //         _record = record;
    //         _attackSelector = attackSelector;
    //         _context = new Boss1Context(new SearchPlayerState(record));
    //         
    //         _attackStates = new IBoss1State[]
    //         {
    //             new TackleState(_record),
    //             new SpitOutState(_record, runner),
    //             new VacuumState(_record),
    //             new ChargeJumpState(_record)
    //         };
    //     }
    //
    //     public void Move()
    //     {
    //         // ここでSearchを使うかもしれない
    //         if (true)
    //         {
    //             // Searchで取得してここでセット
    //             // _context.CurrentState.Move(Transform transform);
    //         }
    //         else
    //         {
    //             // ランダムな動きなど
    //             _context.CurrentState.Move();
    //         }
    //     }
    //     
    //     // このメソッドを呼んだ後にAttackCoolTimeを回す
    //     public void Attack()
    //     {
    //         // ここで絶対にSearchを使う
    //         var units = Search();
    //         if (units.Any())
    //         {
    //             // _context.CurrentState.Attack(units);
    //         }
    //         else
    //         {
    //             // 継続してSearchステート
    //         }
    //     }
    //
    //     // ステートごとに索敵範囲が異なる可能性がある
    //     // ToDo: _context.Search()が同じ結果を返すようにリファクタリングすれば、これはいらない
    //     IEnumerable<IUnitOnAttacked>  Search()
    //     {
    //         var colliders = _context.CurrentState.Search();
    //         return colliders.Select(collider => collider.GetComponent<IUnitOnAttacked>()); // インターフェースでGetComponetできる？
    //     }
    //     
    // }

    public class ForwardEnemyMove: IEnemyMove
    {
        Rigidbody _rd;
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;

        public ForwardEnemyMove(Rigidbody rd)
        {
            _rd = rd;
        }
        public void Move()
        {
            // とりあえず、前に進めて置く
            // ToDo:　ランダムな動きにする
            var input = Vector3.up;
            _rd.AddForce(acceleration * input, ForceMode.Acceleration);

            if (_rd.velocity.magnitude >= maxVelocity)
                _rd.velocity = maxVelocity * _rd.velocity.normalized;
        }
    }
}

