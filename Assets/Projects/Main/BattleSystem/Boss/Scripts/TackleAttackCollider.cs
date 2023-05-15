using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleAttackCollider : AttackCollider
{
    Collider _collider;

    void Start()
    {
        _collider = GetComponent<Collider>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter TackleCollider");
    }
}