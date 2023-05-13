using Cysharp.Threading.Tasks;
using Fusion;
using Main;
using UnityEngine;

namespace Boss
{
    
// 中央集権的なStateパターン
    public interface IBoss1Context
    {
        public IBoss1State CurrentState { get; }
        public void ChangeState(IBoss1State state);
    }

    public interface IBoss1State : IBoss1Move, IBoss1Attack, IBoss1Search
    {
        public void Process(IBoss1Context context);
        public IBoss1Move EnemyMove { get; }
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

        public IBoss1Move EnemyMove => move;
        public IBoss1Attack EnemyAttack => attack;

        protected IBoss1Attack attack;
        protected float attackCoolTime;
        protected IBoss1Move move;
        protected IBoss1Search search;

        protected Boss1AbstractState(Boss1Record record)
        {
            Record = record;
        }

        public abstract void Process(IBoss1Context context);

        public void Move(Vector3 input = default)
        {
            move.Move(input);
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
            move = new WanderingMove(
                new WanderingMove.Record
                {
                    InputSimulationFrequency = 2f
                },
                new SimpleMove(new SimpleMove.Record
                {
                    GameObject = Record.GameObject,
                    Acceleration = 20f,
                    MaxVelocity = 1f
                })
            );
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new DoNothingAttack();
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
            move = new ToTargetMove(
                new ToTargetMove.Record
                {
                    Transform = Record.Transform
                    // Target = Record.TargetBuffer.First() // ToDo: 適当にこれでいいんじゃない？と代入した　→　エラーになった
                }, new SimpleMove(new SimpleMove.Record
                {
                    GameObject = Record.GameObject,
                    Acceleration = 30f,
                    MaxVelocity = 2.5f
                }));
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
        }
    }

    public class ChargeJumpState : Boss1AbstractState
    {
        bool _isCharging;
        bool _isCompleted;

        public ChargeJumpState(Boss1Record record) : base(record)
        {
            move = new LookAtTargetMove(Record.Transform);
            search = new RangeSearch(Record.Transform, Record.SearchRadius,
                LayerMask.GetMask("Player"));
            attack = new DoNothingAttack();
            attackCoolTime = 0;
        }

        public override void Process(IBoss1Context context)
        {
            Debug.Log("ChargeJumpingState.Process()");
            if (_isCompleted)
                context.ChangeState(new JumpState(Record));
            else
                ChargeJump().Forget();
        }

        async UniTaskVoid ChargeJump()
        {
            if (_isCharging) return;
            _isCharging = true;

            for (float t = 0; t < Record.ChargeJumpTime; t += Time.deltaTime) await UniTask.Yield();

            _isCharging = false;
            _isCompleted = true;
        }
    }

    public class JumpState : Boss1AbstractState
    {
        bool _isJumping;
        bool _isCompleted;

        public JumpState(Boss1Record record) : base(record)
        {
            move = new ToTargetMove(
                new ToTargetMove.Record
                {
                    Transform = Record.Transform
                }, new SimpleMove(new SimpleMove.Record
                {
                    GameObject = Record.GameObject,
                    Acceleration = 30f,
                    MaxVelocity = 2.5f
                })
            );
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
                context.ChangeState(new SearchPlayerState(Record));
            else
                Jump().Forget();
        }

        async UniTaskVoid Jump()
        {
            if (_isJumping) return;
            _isJumping = true;

            MoveUtility.Jump(Record.Rd, Record.JumpTime);

            for (float t = 0; t < Record.JumpTime; t += Time.deltaTime) await UniTask.Yield();

            _isJumping = false;
            _isCompleted = true;
        }
    }

    public class SpitOutState : Boss1AbstractState
    {
        public SpitOutState(Boss1Record record, NetworkRunner runner) : base(record)
        {
            move = new LookAtTargetMove(Record.Transform);
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
            move = new LookAtTargetMove(Record.Transform);
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