using Fusion;
using System;
using System.Linq;
using UnityEngine;

namespace Main
{
    [RequireComponent(typeof( NetworkRigidbody))]
public class NetworkEnemyController :  PoolableObject
{
    [SerializeField] NetworkPrefabRef resourcePrefab;
    
    public readonly float acceleration = 10f;
    public readonly float maxVelocity = 8;
    public readonly float resistance = 0.9f; // Determine the slipperiness.

    
    bool isInitialized = false;
    readonly float _detectionRange = 30;
    GameObject _targetPlayerObj;

    public enum EnemyState
    {
        Idle,ChasingPlayer,
    }

    EnemyState _state = EnemyState.Idle;

    Rigidbody _rb;

    public Action onDespawn = () => { };
    
    IUnitMove _move;

    public override void Spawned()
    {
        _rb = GetComponent<Rigidbody>();
        _move = new RegularMove()
        {
            transform = transform,
            rd = _rb,
        };
        isInitialized = true;
    }

    public override void FixedUpdateNetwork()
    {
        if(!HasStateAuthority) return;
        
        switch (_state)
        {
            case EnemyState.Idle:
                Search();
                break;
            case EnemyState.ChasingPlayer:
                Chase();
                break;
        }
    }

    void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, _detectionRange);
        var players = colliders.
            Where(collider => collider.CompareTag("Player")).
            Select(collider => collider.gameObject);
        Debug.Log($"players = {string.Join(",",players)}");
        if (players.Any())
        {
            _targetPlayerObj = players.First();
            _state = EnemyState.ChasingPlayer;
        }
        else
        {
            _state = EnemyState.Idle;
        }
    }

    void Chase()
    {
        var direction = Utility.SetYToZero(_targetPlayerObj.transform.position - gameObject.transform.position).normalized;
        _move.Move(direction);
        // _rb.AddForce(acceleration * directionVec, ForceMode.Acceleration);
        // if (_rb.velocity.magnitude >= maxVelocity)
        //     _rb.velocity = maxVelocity * _rb.velocity.normalized;
        // if (directionVec == Vector3.zero)
        //     _rb.velocity = resistance * _rb.velocity; //Decelerate when there is no key input
    }

    public void OnDefeated()
    {
        Debug.Log($"Runner = {Runner}"); // Todo: シーン切り替え時にRunnerがnullになっている時があるかも
        Runner.Spawn(resourcePrefab, transform.position, Quaternion.identity, PlayerRef.None);
        onDespawn();
        Runner.Despawn(Object);
    }

    void OnDisable()
    {
        OnInactive();
    }
    protected override void OnInactive()
    {
        if (!isInitialized) return;
        _state = EnemyState.Idle;
    }
}

}

