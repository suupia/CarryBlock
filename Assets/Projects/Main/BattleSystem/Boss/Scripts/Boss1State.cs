using System;
using System.Collections.Generic;
using Fusion;
using Main;
using UnityEngine;

public interface IBoss1Context
{
    public IBoss1State CurrentState { get; }
    public void ChangeState(IBoss1State state);
}

public interface IBoss1State : IMove
{
    // ToDo: IAttackやIMoveなどのインターフェースを透過的にしたい
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

    // [SerializeField] public State overrideOnDetectedState;
    [SerializeField] public bool showGizmos;
    [SerializeField] public bool showGUI;

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
    protected Boss1StateGenerator _stateGenerator { get; }

    public Boss1AbstractState(Boss1Record record, Boss1StateGenerator stateGenerator)
    {
        Record = record;
        _stateGenerator = stateGenerator;
    }

    public abstract void Process(IBoss1Context state);

    public abstract void Move(Vector3 input);
}

public class SearchPlayerState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public SearchPlayerState(Boss1Record record, Boss1StateGenerator stateGenerator) : base(record, stateGenerator)
    {
        _attack = new DoNothingAttack();
        _move = new WanderingMove(
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
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
        
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("LostState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
    }
}

public class TackleState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public TackleState(Boss1Record record, Boss1StateGenerator stateGenerator) : base(record, stateGenerator)
    {
        _attack = new ToNearestAttack(new TargetBufferAttack.Context
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
        _attackCoolTime = 1;
        _move = new ToTargetMove(
            new ToTargetMove.Record
            {
                Transform = Record.Transform
            }, new SimpleMove(new SimpleMove.Record
            {
                GameObject = Record.GameObject,
                Acceleration = 30f,
                MaxVelocity = 2.5f
            }));
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
        
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("TacklingState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
    }
}

public class JumpState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public JumpState(Boss1Record record, Boss1StateGenerator stateGenerator) : base(record, stateGenerator)
    {
        _attack = new ToNearestAttack(new TargetBufferAttack.Context
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
        _attackCoolTime = Record.DefaultAttackCoolTime;
        _move = new ToTargetMove(
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
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("JumpingState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
    }
}

public class ChargeJumpState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public ChargeJumpState(Boss1Record record, Boss1StateGenerator stateGenerator) : base(record, stateGenerator)
    {
        _attack = new DoNothingAttack();
        _attackCoolTime = 0;
        _move = new LookAtTargetMove(Record.Transform);
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("ChargeJumpingState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
    }
}

public class SpitOutState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public SpitOutState(NetworkRunner runner, Boss1Record record, Boss1StateGenerator stateGenerator) : base(record,
        stateGenerator)
    {
        _attack = new ToFurthestAttack(new TargetBufferAttack.Context
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
        _attackCoolTime = Record.DefaultAttackCoolTime;
        _move = new LookAtTargetMove(Record.Transform);
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("SpitOutState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
    }
}

public class VacuumState : Boss1AbstractState
{
    IEnemyAttack _attack;
    float _attackCoolTime;
    IMove _move;
    IEnemySearch _search;

    public VacuumState(Boss1Record record, Boss1StateGenerator stateGenerator) : base(record, stateGenerator)
    {
        _attack = new ToNearestAttack(new TargetBufferAttack.Context
            {
                Transform = Record.Transform,
                TargetBuffer = Record.TargetBuffer
            },
            new ToTargetAttack(Record.GameObject, new MockAttack())
        );
        _attackCoolTime = Record.DefaultAttackCoolTime;
        _move = new LookAtTargetMove(Record.Transform);
        _search = new RangeSearch(Record.Transform, Record.SearchRadius,
            LayerMask.GetMask("Player"));
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("VacuumingState.Process()");
    }

    public override void Move(Vector3 input)
    {
        _move.Move(input);
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
    public IBoss1State TacklingState { get; }
    public IBoss1State JumpingState { get; }
    public IBoss1State ChargeJumpingState { get; }
    public IBoss1State SpitOutState { get; }
    public IBoss1State VacuumingState { get; }

    public Boss1StateGenerator(NetworkRunner runner, Boss1Record record)
    {
        LostState = new SearchPlayerState(record, this);
        TacklingState = new TackleState(record, this);
        JumpingState = new JumpState(record, this);
        ChargeJumpingState = new ChargeJumpState(record, this);
        SpitOutState = new SpitOutState(runner, record, this);
        VacuumingState = new VacuumState(record, this);
    }
}

