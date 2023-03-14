using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class OnTriggerEnterComponent : MonoBehaviour
{
    Action<Collider> action = other => { };

    public void SetOnTriggerEnterAction(Action<Collider> action)
    {
        this.action = action;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnterComponent collide gameObject.name:{other.gameObject.name}");
        action(other);
    }
}
