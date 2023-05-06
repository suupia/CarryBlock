using UnityEngine;

namespace Main
{
    public interface IMove
    {
        void Move(Vector3 input);
    }

    public class RegularMove : IMove
    {
        public Transform transform { get; set; }
        public Rigidbody rd { get; set; }
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;
        public float targetRotationTime { get; set; } = 0.2f;
        public float maxAngularVelocity { get; set; } = 100f;


        public void Move(Vector3 input)
        {
            var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
            // Debug.Log($"deltaAngle = {deltaAngle}");

            if (input != Vector3.zero)
            {
                // Rotate if there is a difference of more than Epsilon degrees
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var torque = 2 * deltaAngle / Mathf.Sqrt(targetRotationTime);
                    rd.AddTorque(torque * Vector3.up, ForceMode.Acceleration);
                }

                if (rd.angularVelocity.magnitude >= rd.maxAngularVelocity)
                    rd.angularVelocity = maxAngularVelocity * rd.angularVelocity.normalized;

                rd.AddForce(acceleration * input, ForceMode.Acceleration);

                if (rd.velocity.magnitude >= maxVelocity)
                    rd.velocity = maxVelocity * rd.velocity.normalized;
            }
        }
    }
}