using UnityEngine;

namespace Main
{
    public class CameraFollowTarget : MonoBehaviour
    {
        Vector3 _offset;
        Transform _target;

        void LateUpdate()
        {
            // Position the camera at the target location plus the offset.
            transform.position = _target.position + _offset;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _offset = transform.position - target.position;
        }
    }
}