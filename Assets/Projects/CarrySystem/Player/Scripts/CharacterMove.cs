using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    
    public class CharacterMove : ICharacterMove
    {
        readonly Transform _transform;
        readonly Rigidbody _rb;
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;
        public float targetRotationTime { get; set; } = 0.2f;
        public float maxAngularVelocity { get; set; } = 100f;

        public CharacterMove(Transform transform, Rigidbody rb)
        {
            _transform = transform;
            _rb = rb;
        }

        public void Move(Vector3 input)
        {
            var deltaAngle = Vector3.SignedAngle(_transform.forward, input, Vector3.up);
            // Debug.Log($"deltaAngle = {deltaAngle}");

            if (input != Vector3.zero)
            {
                // Rotate if there is a difference of more than Epsilon degrees
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var torque = (2 * deltaAngle) / Mathf.Sqrt(targetRotationTime);
                    _rb.AddTorque(torque * Vector3.up, ForceMode.Acceleration);
                }

                if (_rb.angularVelocity.magnitude >= _rb.maxAngularVelocity)
                    _rb.angularVelocity = maxAngularVelocity * _rb.angularVelocity.normalized;

                _rb.AddForce(acceleration * input, ForceMode.Acceleration);

                if (_rb.velocity.magnitude >= maxVelocity)
                    _rb.velocity = maxVelocity * _rb.velocity.normalized;
            }
        }
    }
}