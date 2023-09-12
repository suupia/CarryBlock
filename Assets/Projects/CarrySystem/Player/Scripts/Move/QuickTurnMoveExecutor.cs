using Carry.CarrySystem.Player.Interfaces;
using UnityEngine;
using Carry.CarrySystem.Player.Info;
#nullable enable

namespace Carry.CarrySystem.Player.Scripts
{
    public class QuickTurnMoveExecutor : IMoveExecutor
    {
        PlayerInfo _info = null!;
         readonly float _acceleration = 30f;
         readonly float _maxVelocity = 9f;

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
            if (input != Vector3.zero)
            {
                _playerAnimatorPresenter?.Walk();   
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