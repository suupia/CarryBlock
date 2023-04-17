using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerUnitMove
{
    void Move(Vector3 input);
}

public class RegularMove : IPlayerUnitMove
{
    public Transform transform { get; set; }
    public Rigidbody rigidbody { get; set; }
    public float acceleration { get; set; } = 30f;
    public float maxVelocity { get; set; } = 15f;
    public float torque { get; set; } = 1000f;
    public float maxAngularVelocity { get; set; } = 100f;
    

    public void Move(Vector3 input)
    {
        var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
        // Debug.Log($"deltaAngle = {deltaAngle}");

        if (input != Vector3.zero)
        {
            if (!Mathf.Approximately(deltaAngle, 0))
                rigidbody.AddTorque(Mathf.Sign(deltaAngle) * torque * Vector3.up, ForceMode.Acceleration);
            if (rigidbody.angularVelocity.magnitude >= rigidbody.maxAngularVelocity)
                rigidbody.angularVelocity = maxAngularVelocity * rigidbody.angularVelocity.normalized;

            rigidbody.AddForce(acceleration * input, ForceMode.Acceleration);
            if (rigidbody.velocity.magnitude >= maxVelocity)
                rigidbody.velocity = maxVelocity * rigidbody.velocity.normalized;
        }

    }
}