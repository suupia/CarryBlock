using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class OnTriggerEnterComponet : MonoBehaviour
{
    Collider collider;
    Rigidbody rigidbody;
    Action<Collider> action;

    private void Awake()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public void SetOnTriggerEnterAction(Action<Collider> action)
    {
        this.action = action;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnterComponet collide gameObject.name:{other.gameObject.name}");
        action(other);
    }
}
