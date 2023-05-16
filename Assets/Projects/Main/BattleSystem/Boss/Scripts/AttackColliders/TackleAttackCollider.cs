using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Boss
{
    public class TackleAttackCollider : AttackCollider
    {
        Collider _collider;

        void Start()
        {
            _collider = GetComponent<Collider>();
        
        }

        void OnTriggerEnter(Collider other)
        {
            if(!other.gameObject.CompareTag("Player"))return;
        
            Debug.Log("OnTriggerEnter TackleCollider");
        }
    }
}
