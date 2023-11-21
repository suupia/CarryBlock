using UnityEngine;

namespace Carry.GameSystem.Player.Scripts
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
            
            Debug.Log($"transform.position = {transform.position}");
            Debug.Log($"target.position = {target.position}");
            Debug.Log($"_offset = {_offset}");
        }
    }
}