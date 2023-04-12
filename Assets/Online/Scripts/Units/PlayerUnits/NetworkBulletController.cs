using System;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// とりあえずNetworkObjectとする。
/// もし重かったり、無料枠を超えるようなら、TickAlignedのRPCでトリガーだけ通信する方式にする
/// 参考：https://docs.google.com/presentation/d/1kGN7ZEleBgpXuXnUin8y67LmXrmQuAtbgu4rz3QSY6U/edit#slide=id.g1592fa1edef_0_25
/// </summary>
[RequireComponent(typeof(NetworkObject))]
public class NetworkBulletController : NetworkBehaviour, IPoolableObject
{
    bool isInitialized = false;
    Rigidbody _rb;
    
    readonly float _speed = 30;
    readonly float _lifeTime = 5;
    [Networked] TickTimer LifeTimer { get; set; }
    
    
    public void Init(GameObject targetGameObj)
    {
        var directionVec = (targetGameObj.transform.position - transform.position).normalized;
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(_speed*directionVec , ForceMode.Impulse);
        LifeTimer = TickTimer.CreateFromSeconds(Runner, _lifeTime);
        
        isInitialized = true;
    }
    
    // public override void Spawned()
    // {
    //     if (Object.HasStateAuthority)
    //     {
    //         Life = TickTimer.CreateFromSeconds(Runner, 3f);
    //     }
    // }

    public override void FixedUpdateNetwork()
    {
        if (Object.HasStateAuthority)
        {
            if (LifeTimer.Expired(Runner)) Runner.Despawn(Object);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(!HasStateAuthority)return;
        
        if (other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<NetworkEnemyController>();
            if(enemy == null) Debug.LogError("The game object with the 'Enemy' tag does not have the 'NetworkEnemyController' component attached.");;
            enemy.OnDefeated();
            DestroyBullet();
        }
    }
    void DestroyBullet()
    {
        Runner.Despawn(Object);
    }

    void OnDisable()
    {
       OnInactive();
    }

    public void OnInactive()
    {
        if(!isInitialized)return;
        _rb.velocity = Vector3.zero;    
    }

}
