using System.Collections;
using System.Collections.Generic;
using Boss;
using UnityEngine;
using Fusion;
using Main;

# nullable enable

public class SpitOutAttackCollider : NetworkTargetAttackCollider
{

    readonly float _lifeTime = 5;
    readonly float _speed = 30;
    Rigidbody? _rb;
    bool _isInitialized;

    [Networked] TickTimer LifeTimer { get; set; }

    public override void Init(Transform targetTransform)
    {
        Debug.Log($"SpitOut AttackCollider Init");
        Debug.Log($"transform.position:{transform.position} targetTransform.position:{targetTransform.position}");
        var directionVec = (targetTransform.position - transform.position).normalized;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_speed * directionVec, ForceMode.Impulse);
        LifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);

        _isInitialized = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<IPlayerOnAttacked>();
        if (player == null)
        {
            Debug.LogError("The game object with the 'Player' tag does not have the 'IPlayerOnAttacked' component attached.");
            return;
        }

        var afterStats = CalculateDamage(player.NetworkPlayerStruct);
        player.OnAttacked(afterStats);
    }


    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
            if (LifeTimer.Expired(Runner))
                DestroyThisObject();
    }

    void DestroyThisObject()
    {
        Runner.Despawn(Object);
    }

    protected override void OnInactive()
    {
        if (!_isInitialized) return;
        _rb.velocity = Vector3.zero;
    }
    
    NetworkPlayerStruct CalculateDamage(NetworkPlayerStruct playerStruct)
    {
        playerStruct.Hp -= 10;
        return playerStruct;
    }
}