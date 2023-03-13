using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class OnTriggerEnterComponent : MonoBehaviour
{
    Action<Collider> action/* = other => {}*/;

    public void SetOnTriggerEnterAction(Action<Collider> action)
    {
        Debug.Log($"SetAction!!!!!!!!!!");
        this.action = action;
        Debug.Log($"Set Action action:{this.action}");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnterComponent collide gameObject.name:{other.gameObject.name}");
        Debug.Log($"action:{action}");
        Debug.Log($"other:{other}");

        action(other);
    }
}
