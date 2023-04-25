using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Main
{
    public class CameraFollowTarget : MonoBehaviour
    {
        Transform _target;
        Vector3 _offset;

        public void SetTarget(Transform target)
        {
            _target = target;
            _offset = transform.position - target.position;
        }

        void LateUpdate()
        {
            // Position the camera at the target location plus the offset.
            transform.position = _target.position + _offset;
        }
    }
}
