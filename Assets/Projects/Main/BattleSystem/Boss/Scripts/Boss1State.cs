using System.Diagnostics;
using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Boss
{
    
// 中央集権的なStateパターン
    public interface IBoss1Context
    {
        public IBoss1State CurrentState { get; }
        public void ChangeState(IBoss1State state);
    }

    public interface IBoss1State : IEnemyMoveExecutor, IBoss1Attack, IBoss1Search
    {
        public void Process(IBoss1Context context);
        public IEnemyMoveExecutor EnemyMove { get; }
        public IBoss1Attack EnemyAttack { get; }
    }


    public class Boss1Context : IBoss1Context
    {
        public IBoss1State CurrentState { get; private set; }

        public Boss1Context(IBoss1State initState)
        {
            CurrentState = initState;
        }

        public void ChangeState(IBoss1State state)
        {
            CurrentState = state;
        }
    }

    /// <summary>
    ///     Boss1Recordを保持するための抽象クラス
    /// </summary>
    public abstract class Boss1AbstractState : IBoss1State
    {
        protected Boss1Record Record { get; }

        public IEnemyMoveExecutor EnemyMove => move;
        public IBoss1Attack EnemyAttack => attack;

        protected IBoss1Attack attack;
        protected float attackCoolTime;
        protected IEnemyMoveExecutor move;
        protected IBoss1Search search;

        protected Boss1AbstractState(Boss1Record record)
        {
            Record = record;
        }

        public abstract void Process(IBoss1Context context);

        public void Move()
        {
            move.Move();
        }

        public Collider[] Search()
        {
            return search.Search();
        }


        public void Attack()
        {
            attack.Attack();
        }
    }

    public class SearchPlayerState : Boss1AbstractState
    {
        public SearchPlayerState(Boss1Record record) : base(record)
        {
            move = new RandomMove(simulationInterval: 2f, 
                new NonTorqueRegularMove(
                    record.Transform, record.Rb, acceleration: 20f, maxVelocity: 1.0f));
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new DoNothingAttackOld();
            attackCoolTime = 0;
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("SearchPlayerState.Process()");
        }
    }

    public class TackleState : Boss1AbstractState
    {
        public TackleState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));
                search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new ToNearestAttack(new TargetBufferAttack.Context
                {
                    Transform = Record.Transform,
                    TargetBuffer = Record.TargetBuffer
                },
                new ToTargetAttack(
                    Record.GameObject,
                    new RangeAttack(new RangeAttack.Context
                    {
                        Transform = Record.Transform,
                        Radius = 2f,
                        AttackSphereLifeTime = 0.5f
                    })
                )
            );
            attackCoolTime = 1;
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("TacklingState.Process()");
            var targetMove = move as TargetMove;
            Debug.Log($"targetMove.Target: {targetMove.Target}");
        }
    }

    public class ChargeJumpState : Boss1AbstractState
    {
        bool _isCharging;
        bool _isCompleted;
        JumpState _jumpState;

        public ChargeJumpState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new DoNothingAttackOld();
            attackCoolTime = 0;

            _jumpState = new JumpState(record);
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("ChargeJumpingState.Process()");
            if (_isCompleted)
            {
                Reset(); // 状態を持つため、リセットが必須
                context.ChangeState(_jumpState);
            }
            else
            {
                ChargeJump().Forget();
            }
        }

        async UniTaskVoid ChargeJump()
        {
            if (_isCharging) return;
            _isCharging = true;

            for (float t = 0; t < Record.ChargeJumpTime; t += Time.deltaTime) await UniTask.Yield();

            _isCharging = false;
            _isCompleted = true;
        }

        void Reset()
        {
            _isCharging = false;
            _isCompleted = false;
        }
    }

    public class JumpState : Boss1AbstractState
    {
        bool _isJumping;
        bool _isCompleted;

        public JumpState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform,
                new NonTorqueRegularMove(
                    record.Transform, record.Rb, acceleration: 30f, maxVelocity: 2.5f));
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new ToNearestAttack(new TargetBufferAttack.Context
                {
                    Transform = Record.Transform,
                    TargetBuffer = Record.TargetBuffer
                },
                new ToTargetAttack(
                    Record.GameObject,
                    new DelayAttack(
                        Record.JumpTime,
                        new RangeAttack(new RangeAttack.Context
                        {
                            Transform = Record.Transform,
                            Radius = Record.JumpAttackRadius
                        })
                    )
                )
            );
            attackCoolTime = Record.DefaultAttackCoolTime;
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("JumpingState.Process()");
            if (_isCompleted)
            {
                Reset(); // 状態を持つので、リセットが必須　ただし、このクラスは毎回newされているので厳密には不要
                context.ChangeState(new SearchPlayerState(Record));
            }
            else
            {
                Jump().Forget();
            }
        }

        async UniTaskVoid Jump()
        {
            if (_isJumping) return;
            _isJumping = true;

            MoveUtility.Jump(Record.Rb, Record.JumpTime);

            for (float t = 0; t < Record.JumpTime; t += Time.deltaTime) await UniTask.Yield();

            _isJumping = false;
            _isCompleted = true;
        }

        void Reset()
        {
             _isJumping = false;
             _isCompleted = false;
        }
    }

    public class SpitOutState : Boss1AbstractState
    {
        public SpitOutState(Boss1Record record, NetworkRunner runner) : base(record)
        {
            move = new TargetMove(record.Transform,new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new ToFurthestAttack(new TargetBufferAttack.Context
                {
                    Transform = Record.Transform,
                    TargetBuffer = Record.TargetBuffer
                },
                new ToTargetAttack(
                    Record.GameObject,
                    new DelayAttack(
                        Record.ChargeSpitOutTime,
                        new LaunchNetworkObjectAttack(new LaunchNetworkObjectAttack.Context
                            {
                                Runner = runner,
                                From = Record.finSpawnerTransform,
                                Prefab = Resources.Load<GameObject>("Prefabs/Attacks/Fin")
                            }
                        )
                    )
                )
            );
            attackCoolTime = Record.DefaultAttackCoolTime;
            Debug.Log($"Record.finSpawnerTransform = {Record.finSpawnerTransform}");
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("SpitOutState.Process()");
        }
    }

    public class VacuumState : Boss1AbstractState
    {
        public VacuumState(Boss1Record record) : base(record)
        {
            move = new TargetMove(record.Transform, new LookAtInputMoveDecorator(record.Transform, new DoNothingInputMove()));
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new ToNearestAttack(new TargetBufferAttack.Context
                {
                    Transform = Record.Transform,
                    TargetBuffer = Record.TargetBuffer
                },
                new ToTargetAttack(Record.GameObject, new MockAttack())
            );
            attackCoolTime = Record.DefaultAttackCoolTime;
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("VacuumingState.Process()");
        }
    }
}