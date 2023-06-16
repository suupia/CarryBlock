using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using Carry.CarrySystem.Player.Info;

namespace Carry.CarrySystem.Player.Scripts
{
    public class QuickTurnMove : ICharacterMove
    {
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;
        public float rotateTime { get; set; } = 0.2f;
        
        public void Move(PlayerInfo info,Vector3 input)
        {
            var transform = info.playerObj.transform;
            var rb = info.playerRb;
            
            var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
            // Debug.Log($"deltaAngle = {deltaAngle}");

            if (input != Vector3.zero)
            {
                // Rotate if there is a difference of more than Epsilon degrees
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var rotateQuaternion = Quaternion.Euler(0, deltaAngle, 0);
                    rb.MoveRotation(rb.rotation * rotateQuaternion);

                    // ToDo: DoTweenで回転させる
                    // var rotateAngle = new Vector3(0, deltaAngle, 0);
                    // _rb.DORotate(rotateAngle, rotateTime)
                    //     .SetEase(Ease.OutExpo)
                    //     .Play();

                }
                
                // ToDo: _rb.MovePosition()で素早く移動させる
                rb.AddForce(acceleration * input, ForceMode.Acceleration);

                if (rb.velocity.magnitude >= maxVelocity)
                    rb.velocity = maxVelocity * rb.velocity.normalized;
            }
        }
    }

    public class CharacterMove : ICharacterMove
    {
        public float acceleration { get; set; } = 30f;
        public float maxVelocity { get; set; } = 8f;
        public float targetRotationTime { get; set; } = 0.2f;
        public float maxAngularVelocity { get; set; } = 100f;
        

        public void Move(PlayerInfo info, Vector3 input)
        {
            var transform = info.playerObj.transform;
            var rb = info.playerRb;
            
            var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
            // Debug.Log($"deltaAngle = {deltaAngle}");

            if (input != Vector3.zero)
            {
                // Rotate if there is a difference of more than Epsilon degrees
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var torque = (2 * deltaAngle) / Mathf.Sqrt(targetRotationTime);
                    rb.AddTorque(torque * Vector3.up, ForceMode.Acceleration);
                }

                if (rb.angularVelocity.magnitude >= rb.maxAngularVelocity)
                    rb.angularVelocity = maxAngularVelocity * rb.angularVelocity.normalized;

                rb.AddForce(acceleration * input, ForceMode.Acceleration);

                if (rb.velocity.magnitude >= maxVelocity)
                    rb.velocity = maxVelocity * rb.velocity.normalized;
            }
        }
    }
}