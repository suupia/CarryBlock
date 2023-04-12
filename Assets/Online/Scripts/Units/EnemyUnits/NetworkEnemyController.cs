using Fusion;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof( NetworkCharacterControllerPrototype))]
public class NetworkEnemyController : NetworkBehaviour
{
    [SerializeField] NetworkPrefabRef resourcePrefab;
    
    readonly float _detectionRange = 30;

    GameObject _targetPlayerObj;

    public enum EnemyState
    {
        Idle,ChasingPlayer,
    }

    EnemyState _state = EnemyState.Idle;

    NetworkCharacterControllerPrototype _cc;

    public Action onDespawn = () => { };

    public override void Spawned()
    {
        _cc = GetComponent<NetworkCharacterControllerPrototype>();

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
        var directionVec = Utility.SetYToZero(_targetPlayerObj.transform.position - gameObject.transform.position).normalized;
        _cc.Move(directionVec);
    }

    public void OnDefeated()
    {
        Runner.Spawn(resourcePrefab, transform.position, Quaternion.identity, PlayerRef.None);
        onDespawn();
        Runner.Despawn(Object);
    }

    
}
