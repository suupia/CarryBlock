using System.Collections;
using System.Collections.Generic;
using Boss;
using UnityEngine;
using Fusion;
using Main;

# nullable enable

[RequireComponent(typeof(Collider))]
public class SpitOutAttackCollider : NetworkTargetAttackCollider
{

    readonly float _lifeTime = 5;
    readonly float _force = 10;
    Rigidbody? _rb;
    bool _isInitialized;
    readonly int _damage = 1;

    [Networked] TickTimer LifeTimer { get; set; }

    public override void Init(Transform targetTransform)
    {
        var directionVec = (targetTransform.position - transform.position).normalized;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_force * directionVec, ForceMode.Impulse);
        LifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);

        _isInitialized = true;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"other.tag: {other.tag}, other.name: {other.name}");
        if (!other.CompareTag("Player")) return;
        var player = other.GetComponent<IPlayerOnAttacked>();
        if (player == null)
        {
            Debug.LogError("The game object with the 'Player' tag does not have the 'IPlayerOnAttacked' component attached.");
            return;
        }

        player.OnAttacked(_damage);
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
    

}