using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitMove
{
    void Move(Vector3 input);
}

public class RegularMove : IUnitMove
{
    public Transform transform { get; set; }
    public Rigidbody rd { get; set; }
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
            // Rotate if there is a difference of more than 3 degrees
            if (Mathf.Abs(deltaAngle) >= 3.0f)
                rd.AddTorque(Mathf.Sign(deltaAngle) * torque * Vector3.up, ForceMode.Acceleration);

            if (rd.angularVelocity.magnitude >= rd.maxAngularVelocity)
                rd.angularVelocity = maxAngularVelocity * rd.angularVelocity.normalized;

            rd.AddForce(acceleration * input, ForceMode.Acceleration);
            
            if (rd.velocity.magnitude >= maxVelocity)
                rd.velocity = maxVelocity * rd.velocity.normalized;
        }
    }
}