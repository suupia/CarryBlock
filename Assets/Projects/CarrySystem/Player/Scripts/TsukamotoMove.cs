using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

namespace Carry.CarrySystem.Player.Scripts
{
    public class TsukamotoMove : ICharacterMove
    {
        PlayerInfo _info;
        float _acceleration = 60f;
        float _maxVelocity = 16f;
        float _rotateTime  = 0.4f;

        public void Setup(PlayerInfo info)
        {
            _info = info;
            _acceleration = info.acceleration;
            _maxVelocity = info.maxVelocity;
            _rotateTime = info.targetRotationTime;
        }
        public void Move(Vector3 input)
        {
            var transform = _info.playerObj.transform;
            var rb = _info.playerRb;
            
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
                rb.AddForce(_acceleration * input, ForceMode.Acceleration);

                if (rb.velocity.magnitude >= _maxVelocity)
                    rb.velocity = _maxVelocity * rb.velocity.normalized;
            }
        }
    }
    
    }