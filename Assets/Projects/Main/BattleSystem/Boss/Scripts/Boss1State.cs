using System;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;

// 中央集権的なStateパターン
public interface IBoss1Context
{
    public IBoss1State CurrentState { get; }
    public void ChangeState(IBoss1State state);
}

public interface IBoss1State : IEnemyMove, IEnemyAttack, IEnemySearch
{
    public void Process(IBoss1Context state);
}

[Serializable]
public record Boss1Record
{
    // constant fields
    public readonly float JumpTime = 2f;
    public readonly float ChargeJumpTime = 0.5f;
    public readonly float JumpAttackRadius = 3f;
    public readonly float ChargeSpitOutTime = 1.5f;
    public readonly float SearchRadius = 6f;
    public readonly float DefaultAttackCoolTime = 4f;

    [SerializeField] public Transform finSpawnerTransform;

    // target buffer
    public HashSet<Transform> TargetBuffer { get; } = new();
    
    // componets
    public GameObject GameObject { get; }
    public Transform Transform => GameObject.transform;
    public Rigidbody Rd { get; }

    readonly NetworkRunner _runner;

    public Boss1Record(NetworkRunner runner, GameObject gameObject)
    {
        _runner = runner;
        GameObject = gameObject;
        Rd = gameObject.GetComponent<Rigidbody>();
    }
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

    protected IEnemyAttack attack;
    protected float attackCoolTime;
    protected IEnemyMove move;
    protected IEnemySearch search;

    protected Boss1AbstractState(Boss1Record record)
    {
        Record = record;
    }

    public abstract void Process(IBoss1Context state);

    public void Move(Vector3 input)
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

    public override void Process(IBoss1Context state)
    {
        Debug.Log("LostState.Process()");
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

    public override void Process(IBoss1Context state)
    {
        Debug.Log("TacklingState.Process()");
    }
}

public class JumpState : Boss1AbstractState
{
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

    public override void Process(IBoss1Context state)
    {
        Debug.Log("JumpingState.Process()");
    }
    
}

public class ChargeJumpState : Boss1AbstractState
{
    public ChargeJumpState(Boss1Record record) : base(record)
    {
        move = new LookAtTargetMove(Record.Transform);
        search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
        attack = new DoNothingAttack();
        attackCoolTime = 0;
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("ChargeJumpingState.Process()");
    }
    
}

public class SpitOutState : Boss1AbstractState
{
    public SpitOutState(NetworkRunner runner, Boss1Record record) : base(record)
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
    }

    public override void Process(IBoss1Context state)
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

    public override void Process(IBoss1Context state)
    {
        Debug.Log("VacuumingState.Process()");
    }
    
}


/// <summary>
///     A class to treat each state as a singleton.
/// </summary>
public class Boss1StateGenerator
{
    // ToDo: FactoryMethodかAbstractFactoryパターンっぽいが厳密には多分違う
    // インターフェイスを切っていないため、クライアントコードが具象のFactoryに依存してしまうが
    // 戻り値がインターフェイスであるため、クライアントコードは抽象のIBoss1Stateに依存することになる
    // もっと厳密にやるにはクライアントコードでDIする必要があるが、Runnerを使っているのでできない。
    public IBoss1State LostState { get; }
    public IBoss1State TackleState { get; }
    public IBoss1State JumpState { get; }
    public IBoss1State ChargeJumpState { get; }
    public IBoss1State SpitOutState { get; }
    public IBoss1State VacuumState { get; }

    public Boss1StateGenerator(NetworkRunner runner, Boss1Record record)
    {
        LostState = new SearchPlayerState(record);
        TackleState = new TackleState(record);
        JumpState = new JumpState(record);
        ChargeJumpState = new ChargeJumpState(record);
        SpitOutState = new SpitOutState(runner, record);
        VacuumState = new VacuumState(record);
    }
}

