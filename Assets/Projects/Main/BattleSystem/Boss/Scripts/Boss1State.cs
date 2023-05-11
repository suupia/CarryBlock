using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    public void Process(IBoss1Context context);
    public IEnemyMove EnemyMove { get; }
    public IEnemyAttack EnemyAttack { get; }
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
    public HashSet<Transform> TargetBuffer { get; set; } = new();
    
    // componets
    public GameObject GameObject { get; private set; } // NetworkControllerのGameObject
    public Transform Transform => GameObject.transform;
    public Rigidbody Rd { get; private set; }

    NetworkRunner _runner;

    // This record will initialize by SerializeField
    Boss1Record()
    {
    }

    public void Init(NetworkRunner runner, GameObject gameObject)
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

    public IEnemyMove EnemyMove => move;
    public IEnemyAttack EnemyAttack => attack;

    protected IEnemyAttack attack;
    protected float attackCoolTime;
    protected IEnemyMove move;
    protected IEnemySearch search;

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
        Debug.Log($"Record.GameObject = {Record.GameObject}");
        Debug.Log($"Record.GameObject.transform = {Record.GameObject.transform}");
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
            ChargeJump();
    }

    async void ChargeJump()
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
            Jump();
    }

    async void Jump()
    {
        if (_isJumping) return;
        _isJumping = true;

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


