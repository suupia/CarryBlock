using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    Transform target;
    Vector3 offset;

    public void SetTarget(Transform target)
    {
        this.target = target;
        offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        // Position the camera at the target location plus the offset.
        transform.position = target.position + offset;
    }
}