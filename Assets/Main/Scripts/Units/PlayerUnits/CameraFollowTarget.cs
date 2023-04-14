using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;


public class CameraFollowTarget : MonoBehaviour
{
    Transform _target;
    Vector3 _offset;

    public void SetTarget(Transform target)
    {
        this._target = target;
        _offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Position the camera at the target location plus the offset.
        transform.position = _target.position + _offset;
    }
}