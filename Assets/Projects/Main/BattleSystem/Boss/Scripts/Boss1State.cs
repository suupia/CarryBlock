using Main;
using UnityEngine;

public interface IBoss1Context
{
    public IBoss1State CurrentState { get; }
    public void ChangeState(IBoss1State state);
}

public interface IBoss1State
{
    // ToDo: IAttackやIMoveなどのインターフェースを透過的にしたい
    public void Process(IBoss1Context state);
}

public record Boss1Record
{
    // constant fields
    public readonly float JumpTime = 2f;
    public readonly float ChargeJumpTime = 0.5f;
    public readonly float JumpAttackRadius = 3f;
    public readonly float ChargeSpitOutTime = 1.5f;
    public readonly float SearchRadius = 6f;
    public readonly float DefaultAttackCoolTime = 4f;

    // componets
    public GameObject GameObject { get; }
    public Transform Transform => GameObject.transform;
    public Rigidbody Rd { get; }

    public Boss1Record(GameObject gameObject)
    {
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

    public Boss1AbstractState(Boss1Record record)
    {
        Record = record;
    }

    public abstract void Process(IBoss1Context state);
}

public class NoneState : IBoss1State
{
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public NoneState(Boss1Record record)
    {
        _attack = default; // ToDO: Nullオブジェクトを代入する
        _move = default;
        _search = default;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class LostState : Boss1AbstractState
{
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public LostState(Boss1Record record) : base(record)
    {
        _attack = default;
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
        ;
    }

    public override void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class TacklingState
{
    Boss1Record _record;
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public TacklingState(Boss1Record record, IAttack attack, IMove move, ISearch search)
    {
        _record = record;
        _attack = attack;
        _move = move;
        _search = search;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class JumpingState
{
    Boss1Record _record;
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public JumpingState(Boss1Record record, IAttack attack, IMove move, ISearch search)
    {
        _record = record;
        _attack = attack;
        _move = move;
        _search = search;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class ChargeJumpingState
{
    Boss1Record _record;
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public ChargeJumpingState(Boss1Record record, IAttack attack, IMove move, ISearch search)
    {
        _record = record;
        _attack = attack;
        _move = move;
        _search = search;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class SpitOutState
{
    Boss1Record _record;
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public SpitOutState(Boss1Record record, IAttack attack, IMove move, ISearch search)
    {
        _record = record;
        _attack = attack;
        _move = move;
        _search = search;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}

public class VacuumingState
{
    Boss1Record _record;
    IAttack _attack;
    float _attackCoolTime;
    IMove _move;
    ISearch _search;

    public VacuumingState(Boss1Record record, IAttack attack, IMove move, ISearch search)
    {
        _record = record;
        _attack = attack;
        _move = move;
        _search = search;
    }

    public void Process(IBoss1Context state)
    {
        Debug.Log("NoneState.Process()");
    }
}