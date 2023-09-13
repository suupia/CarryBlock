using Carry.CarrySystem.Player.Info;
using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;

#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class DashMoveExecutor : IMoveExecutor
    {
        PlayerInfo _info = null!;
        readonly float _acceleration = 100f;
        readonly float _maxVelocity = 27f; // CorrectlyStopの3倍
        readonly float _stoppingForce = 5f;
        
        IPlayerAnimatorPresenter? _playerAnimatorPresenter;

        public void Setup(PlayerInfo info)
        {
            _info = info;
        }

        public void Move(Vector3 input)
        {
            var transform = _info.PlayerObj.transform;
            var rb = _info.PlayerRb;
            var deltaAngle = Vector3.SignedAngle(transform.forward, input, Vector3.up);
            
            if (input != Vector3.zero)
            {
                if (Mathf.Abs(deltaAngle) >= float.Epsilon)
                {
                    var rotateQuaternion = Quaternion.Euler(0, deltaAngle, 0);
                    rb.MoveRotation(rb.rotation * rotateQuaternion);
                }

                rb.AddForce(_acceleration * input, ForceMode.Acceleration);

                if (rb.velocity.magnitude >= _maxVelocity)
                    rb.velocity = _maxVelocity * rb.velocity.normalized;
            }
            else
            {
                // Stop if there is no key input
                // Define 0 < _stoppingForce < 1
                float reductionFactor = Mathf.Max(0f, 1f - _stoppingForce * Time.deltaTime);
                rb.velocity *= Mathf.Pow(reductionFactor, rb.velocity.magnitude);
            }
            
            if (input != Vector3.zero)
            {
                _playerAnimatorPresenter?.Dash();   
            }
            else
            {
                _playerAnimatorPresenter?.Idle();
            }
        }
        
        // Animator
        public void SetPlayerAnimatorPresenter(IPlayerAnimatorPresenter presenter)
        {
            _playerAnimatorPresenter = presenter;
        }
    }
}